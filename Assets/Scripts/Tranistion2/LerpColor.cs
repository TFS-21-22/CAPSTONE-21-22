using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpColor : MonoBehaviour
{
    MeshRenderer cubeMeshRenderer;
    [SerializeField] [Range(0f, 2f)] float lerpTime;

    [SerializeField] Color mycolor;

    public float score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        cubeMeshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (score > 0)
        cubeMeshRenderer.material.color = Color.Lerp(cubeMeshRenderer.material.color, Color.red, lerpTime);

        else
            cubeMeshRenderer.material.color = Color.Lerp(cubeMeshRenderer.material.color, Color.blue, lerpTime);

        if (Input.GetButtonDown("Fire1"))
            score += 1;

        if (Input.GetButtonDown("Fire2"))
            score -= 1;
    }
}
