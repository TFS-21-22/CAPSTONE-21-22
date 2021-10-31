using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transition2 : MonoBehaviour
{
    // Blends between two materials

    public float score;
    public float scoreMultiplyer = 1;
    public Material material1;
    public Material material2;
    float lerpDuration = 2.0f;
    float lerpTime = 0;
    bool dreamState = false;
    Renderer rend;

    public ScoreSystem scoreSystem;
    void Start()
    {
        rend = GetComponent<Renderer>();

        // At start, use the first material
        rend.material = material1;
    }

    void Update()
    {
        if (scoreMultiplyer > 1 && !dreamState)
        {
            lerpTime = 0;
            StartCoroutine(MatLerp(material1, material2));
            dreamState = true;
            // MaterialLerp();

            // ping-pong between the materials over the duration
            //float lerp = Mathf.PingPong(Time.time, duration) / duration;
            //rend.material.Lerp(material1, material2, lerpTime / duration);
            //lerpTime += Time.deltaTime;
            
        }

        if(scoreMultiplyer <= 1 && dreamState == true)
        {
            lerpTime = 0;
            StartCoroutine(MatLerp(material2, material1));
            dreamState = false;
            //float lerp = Mathf.PingPong(Time.time, duration) / duration;
            //rend.material.Lerp(material2, material1, lerp);
        } 
  
        if (Input.GetButtonDown("Fire1"))
            score += 1;
        


        
        if (Input.GetButtonDown("Fire2"))
            score -= 1;

        //score = scoreSystem.score;
        //scoreMultiplyer = scoreSystem.scoreMulitplyer;
    }
    /*
    public void MaterialLerp()
    {
        float lerp = Time.time / 20;
        rend.material.Lerp(material1, material2, lerp);
    }
    */
    IEnumerator MatLerp(Material material1, Material material2)
    {
        lerpTime = 0;

        while(lerpTime < lerpDuration)
        {
            rend.material.Lerp(material1, material2, lerpTime / lerpDuration);
            lerpTime += Time.deltaTime;
            yield return null;
        }
        rend.material = material2;
        
    }

}
