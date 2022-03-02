using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public float score;
    public float scoreMultiplyer = 1;



    public Material material1;
    public Material material2;

    float lerpDuration = 5.0f;
    float lerpTime = 0f;

    public bool dreamState = false;
    bool changedState = true;
    bool objectInView = false;

    Renderer rend;



    public GameObject player;


    public ScoreSystem scoreSystem;

    public Color startColor;
    public Color endColor;

    void Start()
    {

        rend = GetComponent<Renderer>();

        // At start, use the first material
        rend.material.color = startColor;


    }

    void Update()
    {

        if (scoreMultiplyer >= 1.0f && !dreamState)
        {
            dreamState = true;

        }

        if (scoreMultiplyer <= 1 && dreamState == true)
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
            //if(rend.sharedMaterial)
            //{
            //    if (rend.sharedMaterial.mainTexture == material1.mainTexture)
            //        changedState = false;

            //    if (rend.sharedMaterial.mainTexture == material2.mainTexture)
            //        changedState = true;

            //  //  MaterialLerp(material1, material2);
            //}
           // ColorLerp(endColor, startColor);

            if(rend.material.color == startColor)
            {
                
                changedState = false;
            }

            if(rend.material.color == endColor)
            {
                changedState = true;
            }

            ColorLerp(startColor, endColor);

        }
        else
        {
            //if(rend.sharedMaterial && material2)
            //{
            //    if (rend.sharedMaterial.mainTexture == material2.mainTexture)
            //        changedState = false;

            //    if (rend.sharedMaterial.mainTexture == material1.mainTexture)
            //        changedState = true;

            //   // MaterialLerp(material2, material1);
            //}
            //ColorLerp(startColor, endColor);

            if (rend.material.color == endColor)
            {

                changedState = false;
            }

            if (rend.material.color == startColor)
            {
                changedState = true;
            }

            ColorLerp(endColor, startColor);
        }

        if (scoreSystem)
        {
            score = scoreSystem.score;
            scoreMultiplyer = scoreSystem.scoreMulitplyer;
        }


        if (player)
        {
            if ((player.transform.position.z + 10.0f) >= gameObject.transform.position.z)
            {
                objectInView = true;
            }
            else
            {
                objectInView = false;
            }

            if (!objectInView && !gameObject.CompareTag("DNDP"))
            {
                transform.parent.gameObject.SetActive(false);
            }

            if (!objectInView && gameObject.CompareTag("Log"))
            {
                gameObject.SetActive(false);
            }
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

    public void ColorLerp(Color _startColor, Color _endColor)
    {
        if (!changedState)
        {
            lerpTime += Time.deltaTime;

            GetComponent<Renderer>().material.color = Color.Lerp(_startColor, _endColor, lerpTime / lerpDuration);
        }

        if (lerpTime > lerpDuration)
        {
            lerpTime = 0;
            changedState = true;
        }
    }


}
