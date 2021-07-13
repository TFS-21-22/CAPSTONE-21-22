using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transition2 : MonoBehaviour
{
    // Blends between two materials

    public float score;
    public Material material1;
    public Material material2;
    float duration = 2.0f;
    Renderer rend;

    void Start()
    {
        score = 0;
        rend = GetComponent<Renderer>();

        // At start, use the first material
        rend.material = material1;
    }

    void Update()
    {
        if (score > 0)
        {

           // MaterialLerp();
            
            // ping-pong between the materials over the duration
            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            rend.material.Lerp(material1, material2, lerp);
            
        }

        /*else 
        {
            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            rend.material.Lerp(material2, material1, lerp);
        } */
  
        if (Input.GetButtonDown("Fire1"))
            score += 1;
        

        if (Input.GetButtonDown("Fire2"))
            score -= 1;
    }

    public void MaterialLerp()
    {
        float lerp = Time.time / 20;
        rend.material.Lerp(material1, material2, lerp);
    }


}
