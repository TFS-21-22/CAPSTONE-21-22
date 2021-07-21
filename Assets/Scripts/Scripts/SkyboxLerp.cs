using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxLerp : MonoBehaviour
{

    public float score;
    public Material material1;
    public Material material2;
    float lerpDuration = 10.0f;
    float lerpTime = 0;
    bool dreamState = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (score > 0 && !dreamState)
        {
            lerpTime = 0;
            StartCoroutine(MatLerp(material1, material2));
            dreamState = true;
        }

        if (score <= 0 && dreamState == true)
        {
            lerpTime = 0;
           
            StartCoroutine(MatLerp(material2, material1));
            dreamState = false;
        }

        if (Input.GetButtonDown("Fire1"))
            score += 1;

        if (Input.GetButtonDown("Fire2"))
            score -= 1;
    }

    IEnumerator MatLerp(Material material1, Material material2)
    {
        lerpTime = 0;

        while (lerpTime < lerpDuration)
        {
            RenderSettings.skybox.Lerp(material1, material2, lerpTime / lerpDuration);
            lerpTime += Time.deltaTime;
            yield return null;
        }
        //RenderSettings.skybox = material2;
    }
}
