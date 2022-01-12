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
    float lerpDuration = 5.0f;
    float lerpTime = 0;
    bool dreamState = false;
    bool changedState = true;
    bool objectInView = false;
    Renderer rend;

    public GameObject player;


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
            //lerpTime = 0;
            //StartCoroutine(MatLerp(material1, material2));
            dreamState = true;
            //MaterialLerp(material1, material2);

            ////ping - pong between the materials over the duration
            //float lerp = Mathf.PingPong(Time.time, duration) / duration;
            //rend.material.Lerp(material1, material2, lerpTime / duration);
            //lerpTime += Time.deltaTime;

        }

        if(scoreMultiplyer <= 1 && dreamState == true)
        {
            //lerpTime = 0;
            //StartCoroutine(MatLerp(material2, material1));
            dreamState = false;
            //MaterialLerp(material2, material1);
        } 
  
        if (Input.GetButtonDown("Fire1"))
            score += 1;
        


        
        if (Input.GetButtonDown("Fire2"))
            score -= 1;


        if (dreamState)
        {
            if (rend.material.mainTexture == material1.mainTexture)
                changedState = false;

            if (rend.material.mainTexture == material2.mainTexture)
                changedState = true;

            MaterialLerp(material1, material2);
        }
        else
        {
            if (rend.material.mainTexture == material2.mainTexture)
                changedState = false;

            if (rend.material.mainTexture == material1.mainTexture)
                changedState = true;

            MaterialLerp(material2, material1);
        }

        score = scoreSystem.score;
        scoreMultiplyer = scoreSystem.scoreMulitplyer;

        if((player.transform.position.z + 10.0f) >= gameObject.transform.position.z)
        {
            objectInView = true;          
        }
        else
        {
            objectInView = false;
        }

        if(!objectInView)
        {
            transform.parent.gameObject.SetActive(false);
        }
     
    }
    
    public void MaterialLerp(Material _mat1, Material _mat2)
    { 
        if (!changedState)
        {
            lerpTime += Time.deltaTime;
           // float lerp = lerpTime / lerpDuration;
            rend.material.Lerp(_mat1, _mat2, lerpTime / lerpDuration);
        }

        if (lerpTime > lerpDuration)
        {
            lerpTime = 0;
            changedState = true;
            rend.material = _mat2;
        }

    }
    
    //IEnumerator MatLerp(Material material1, Material material2)
    //{
    //    lerpTime = 0;

    //    while(lerpTime < lerpDuration)
    //    {
    //        rend.material.Lerp(material1, material2, lerpTime / lerpDuration);
    //        lerpTime += Time.deltaTime;
           
    //    }
    //    rend.material = material2;

    //    yield return null;
    //}




}
