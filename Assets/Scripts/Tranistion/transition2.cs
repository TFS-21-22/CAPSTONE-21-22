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
        rend.sharedMaterial = material1;

        

    }

    void Update()
    {
       
        if (scoreMultiplyer >= 30.0f && !dreamState)
        {
            dreamState = true;        

        }

        if(scoreMultiplyer <= 1 && dreamState == true)
        {
            //lerpTime = 0;
            //StartCoroutine(MatLerp(material2, material1));
            dreamState = false;
            //MaterialLerp(material2, material1);
        } 
  
        if (Input.GetButtonDown("Fire1"))
            score += 30;
        


        
        if (Input.GetButtonDown("Fire2"))
            score -= 30;


        if (dreamState)
        {
            if (rend.sharedMaterial.mainTexture == material1.mainTexture)
                changedState = false;

            if (rend.sharedMaterial.mainTexture == material2.mainTexture)
                changedState = true;

            MaterialLerp(material1, material2);
        }
        else
        {
            if (rend.sharedMaterial.mainTexture == material2.mainTexture)
                changedState = false;

            if (rend.sharedMaterial.mainTexture == material1.mainTexture)
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

        if(!objectInView && !gameObject.CompareTag("Ground"))
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
            rend.sharedMaterial.Lerp(_mat1, _mat2, lerpTime / lerpDuration);
        }

        if (lerpTime > lerpDuration)
        {
            lerpTime = 0;
            changedState = true;
            rend.sharedMaterial = _mat2;
        }

    }
    
  


}
