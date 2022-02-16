//------------------------------------------------------------------------------------------------------------------
// Volumetric Fog & Mist 2
// Created by Kronnect
//------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace VolumetricFogAndMist2
{
    public class VolumetricFogRenderFeature : ScriptableRendererFeature
    {
        static class ShaderParams
        {
            public static int lightBuffer = Shader.PropertyToID("_LightBuffer");
            public static int mainTex = Shader.PropertyToID("_MainTex");
            public static int blurRT = Shader.PropertyToID("_BlurTex");
            public static int blurRT2 = Shader.PropertyToID("_BlurTex2");
            public static int blurScale = Shader.PropertyToID("_BlurScale");
            public static int forcedInvisible = Shader.PropertyToID("_ForcedInvisible");
            public static int downsampledDepth = Shader.PropertyToID("_DownsampledDepth");
        }

        static int GetScaledSize(int size, float factor) {
            size = (int)(size / factor);
            size /= 4;
            if (size < 1)
                size = 1;
            return size * 4;
        }

        class VolumetricFogRenderPass : ScriptableRenderPass
        {

            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.transparent, -1);
            readonly List<ShaderTagId> shaderTagIdList = new List<ShaderTagId>();
            const int renderingLayer = 1<<50;
            const string m_ProfilerTag = "Volumetric Fog Light Buffer Rendering";

            public void Setup(VolumetricFogRenderFeature settings)
            {
                renderPassEvent = settings.renderPassEvent;
                shaderTagIdList.Clear();
                shaderTagIdList.Add(new ShaderTagId("UniversalForward"));
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                RenderTextureDescriptor lightBufferDesc = cameraTextureDescriptor;
                VolumetricFogManager manager = VolumetricFogManager.GetManagerIfExists();
                if (manager != null && manager.downscaling > 1f) {
                    int size = GetScaledSize(cameraTextureDescriptor.width, manager.downscaling);
                    lightBufferDesc.width = size;
                    lightBufferDesc.height = size;
                }
                lightBufferDesc.colorFormat = RenderTextureFormat.ARGB32;
                lightBufferDesc.depthBufferBits = 0;
                lightBufferDesc.useMipMap = false;
                lightBufferDesc.msaaSamples = 1;
                cmd.GetTemporaryRT(ShaderParams.lightBuffer, lightBufferDesc, FilterMode.Bilinear);
                RenderTargetIdentifier lightBuffer = new RenderTargetIdentifier(ShaderParams.lightBuffer, 0, CubemapFace.Unknown, -1);
                ConfigureTarget(lightBuffer);
                ConfigureClear(ClearFlag.Color, new Color(0, 0, 0, 0));
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                VolumetricFogManager manager = VolumetricFogManager.GetManagerIfExists();

                CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
                cmd.SetGlobalFloat(ShaderParams.forcedInvisible, 0);
                context.ExecuteCommandBuffer(cmd);

                if (manager == null || (manager.downscaling <= 1f && manager.blurPasses < 1))
                {
                    CommandBufferPool.Release(cmd);
                    return;
                }

                foreach (VolumetricFog vg in VolumetricFog.volumetricFogs) {
                    if (vg != null) {
                        vg.meshRenderer.renderingLayerMask = renderingLayer;
                    }
                }

                cmd.Clear();

                var sortFlags = SortingCriteria.CommonTransparent;
                var drawSettings = CreateDrawingSettings(shaderTagIdList, ref renderingData, sortFlags);
                var filterSettings = filteringSettings;
                filterSettings.layerMask = 1 << manager.fogLayer;
                filterSettings.renderingLayerMask = renderingLayer;

                context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filterSettings);

                RenderTargetIdentifier lightBuffer = new RenderTargetIdentifier(ShaderParams.lightBuffer, 0, CubemapFace.Unknown, -1);
                cmd.SetGlobalTexture(ShaderParams.lightBuffer, lightBuffer);

                CommandBufferPool.Release(cmd);

            }

            /// Cleanup any allocated resources that were created during the execution of this render pass.
            public override void FrameCleanup(CommandBuffer cmd)
            {
            }
        }


        class BlurRenderPass : ScriptableRenderPass
        {

            enum Pass
            {
                BlurHorizontal = 0,
                BlurVertical = 1,
                BlurVerticalAndBlend = 2,
                Blend = 3,
                DownscaleDepth = 4,
                BlurVerticalFinal = 5
            }

            ScriptableRenderer renderer;
            Material mat;
            RenderTextureDescriptor rtSourceDesc;
            static Matrix4x4 matrix4x4identity = Matrix4x4.identity;

            public void Setup(Shader shader, ScriptableRenderer renderer, VolumetricFogRenderFeature settings)
            {
                this.renderPassEvent = settings.renderPassEvent;
                this.renderer = renderer;
                if (mat == null)
                {
                    mat = CoreUtils.CreateEngineMaterial(shader);
                }
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                rtSourceDesc = cameraTextureDescriptor;
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {

                VolumetricFogManager manager = VolumetricFogManager.GetManagerIfExists();
                if (manager == null || (manager.downscaling <= 1f && manager.blurPasses < 1))
                {
                    Cleanup();
                    return;
                }

                mat.SetFloat(ShaderParams.blurScale, manager.blurSpread);

                RenderTargetIdentifier source = renderer.cameraColorTarget;

                var cmd = CommandBufferPool.Get("Volumetric Fog Render Feature");

                cmd.SetGlobalFloat(ShaderParams.forcedInvisible, 1);

                RenderTextureDescriptor rtBlurDesc = rtSourceDesc;
                int size = GetScaledSize(rtSourceDesc.width, manager.downscaling);
                rtBlurDesc.width = size;
                rtBlurDesc.height = size;
                rtBlurDesc.colorFormat = RenderTextureFormat.ARGB32;
                rtBlurDesc.useMipMap = false;
                rtBlurDesc.msaaSamples = 1;
                rtBlurDesc.depthBufferBits = 0;

                bool usingDownscaling = manager.downscaling > 1f;
                if (usingDownscaling) {
                    RenderTextureDescriptor rtDownscaledDepth = rtBlurDesc;
                    rtDownscaledDepth.colorFormat = RenderTextureFormat.RFloat;
                    cmd.GetTemporaryRT(ShaderParams.downsampledDepth, rtDownscaledDepth, FilterMode.Bilinear);
                    FullScreenBlit(cmd, source, ShaderParams.downsampledDepth, mat, (int)Pass.DownscaleDepth);
                }

                if (manager.blurPasses < 1) {
                    FullScreenBlit(cmd, ShaderParams.lightBuffer, source, mat, (int)Pass.Blend);
                } else {
                    rtBlurDesc.width = GetScaledSize(rtSourceDesc.width, manager.blurDownscaling);
                    rtBlurDesc.height = GetScaledSize(rtSourceDesc.height, manager.blurDownscaling);
                    cmd.GetTemporaryRT(ShaderParams.blurRT, rtBlurDesc, FilterMode.Bilinear);
                    cmd.GetTemporaryRT(ShaderParams.blurRT2, rtBlurDesc, FilterMode.Bilinear);
                    FullScreenBlit(cmd, ShaderParams.lightBuffer, ShaderParams.blurRT, mat, (int)Pass.BlurHorizontal);
                    for (int k = 0; k < manager.blurPasses - 1; k++) {
                        FullScreenBlit(cmd, ShaderParams.blurRT, ShaderParams.blurRT2, mat, (int)Pass.BlurVertical);
                        FullScreenBlit(cmd, ShaderParams.blurRT2, ShaderParams.blurRT, mat, (int)Pass.BlurHorizontal);
                    }
                    if (usingDownscaling) {
                        FullScreenBlit(cmd, ShaderParams.blurRT, ShaderParams.blurRT2, mat, (int)Pass.BlurVerticalFinal);
                        FullScreenBlit(cmd, ShaderParams.blurRT2, source, mat, (int)Pass.Blend);
                    } else {
                        FullScreenBlit(cmd, ShaderParams.blurRT, source, mat, (int)Pass.BlurVerticalAndBlend);
                    }

                    cmd.ReleaseTemporaryRT(ShaderParams.blurRT2);
                    cmd.ReleaseTemporaryRT(ShaderParams.blurRT);
                }
                cmd.ReleaseTemporaryRT(ShaderParams.lightBuffer);
                if (usingDownscaling) {
                    cmd.ReleaseTemporaryRT(ShaderParams.downsampledDepth);
                }
                context.ExecuteCommandBuffer(cmd);

                CommandBufferPool.Release(cmd);

            }

            void FullScreenBlit(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, int passIndex)
            {
                destination = new RenderTargetIdentifier(destination, 0, CubemapFace.Unknown, -1);
                cmd.SetRenderTarget(destination);
                cmd.SetGlobalTexture(ShaderParams.mainTex, source);
                cmd.DrawMesh(RenderingUtils.fullscreenMesh, matrix4x4identity, material, 0, passIndex);
            }

            /// Cleanup any allocated resources that were created during the execution of this render pass.
            public override void FrameCleanup(CommandBuffer cmd)
            {
            }


            public void Cleanup()
            {
                CoreUtils.Destroy(mat);
                Shader.SetGlobalFloat(ShaderParams.forcedInvisible, 0);
            }

        }

        [SerializeField, HideInInspector]
        Shader shader;
        VolumetricFogRenderPass fogRenderPass;
        BlurRenderPass blurRenderPass;
        public static bool installed;

        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;

        void OnDisable()
        {
            installed = false;
            if (blurRenderPass != null)
            {
                blurRenderPass.Cleanup();
            }
        }

        public override void Create()
        {
            name = "Volumetric Fog 2";
            fogRenderPass = new VolumetricFogRenderPass();
            blurRenderPass = new BlurRenderPass();
            shader = Shader.Find("Hidden/VolumetricFog2/Blur");
            if (shader == null)
            {
                Debug.LogWarning("Could not load Volumetric Fog blur shader.");
            }
        }

        // This method is called when setting up the renderer once per-camera.
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            Camera cam = renderingData.cameraData.camera;
            if (cam.targetTexture != null && cam.targetTexture.format == RenderTextureFormat.Depth) return; // ignore occlusion cams!

            fogRenderPass.Setup(this);
            blurRenderPass.Setup(shader, renderer, this);
            renderer.EnqueuePass(fogRenderPass);
            renderer.EnqueuePass(blurRenderPass);
            installed = true;
        }
    }
}
