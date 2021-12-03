using UnityEngine;
using VolumetricFogAndMist2;

namespace VoluemtricFogAndMist2.Demos {

    public class CapsuleController : MonoBehaviour {

        public VolumetricFog fogVolume;
        public float moveSpeed = 10f;
        public float fogHoleRadius = 5f;
        public float clearDuration = 0.5f;

        Vector3 lastPos = new Vector3(float.MaxValue, 0, 0);

        void Update() {

            float disp = Time.deltaTime * moveSpeed;

            // moves capsule with arrow keys
            if (Input.GetKey(KeyCode.LeftArrow)) {
                transform.Translate(-disp, 0, 0);
            } else if (Input.GetKey(KeyCode.RightArrow)) {
                transform.Translate(disp, 0, 0);
            }
            if (Input.GetKey(KeyCode.UpArrow)) {
                transform.Translate(0, 0, disp);
            } else if (Input.GetKey(KeyCode.DownArrow)) {
                transform.Translate(0, 0, -disp);
            }

            // do not call SetFogOfWarAlpha every frame; only when capsule moves
            if ((transform.position - lastPos).magnitude > 5f) {
                lastPos = transform.position;
                fogVolume.SetFogOfWarAlpha(transform.position, fogHoleRadius, 0, clearDuration);
            }

        }
    }

}