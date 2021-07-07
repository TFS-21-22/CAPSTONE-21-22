using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class WaterPhysics : MonoBehaviour
{
    MeshFilter mf;
    public WaveManager wm;

    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3[] vertices = mf.mesh.vertices;//Grabs meshes vertices

        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = wm.WaveHeight(transform.position.x + vertices[i].x, transform.position.z + vertices[i].z);
        }

        mf.mesh.vertices = vertices;
        mf.mesh.RecalculateNormals();
        mf.mesh.RecalculateBounds();
    }
}
