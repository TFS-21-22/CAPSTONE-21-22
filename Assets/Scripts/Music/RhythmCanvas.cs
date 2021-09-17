using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EBeatScore
{
    missed,
    early,
    ok,
    good,
    perfect
}


public class RhythmCanvas : MonoBehaviour
{
    public Image xButton;   //"X" button image
    public Image xCircle;   //"X" image outer circle

    [SerializeField] Text rhythmText;                   //Prints beat score
    [SerializeField] SmoothCameraScript smoothCamera;   //Camera Smoothing

    Vector3 xScale = new Vector3(3, 3, 3);              //"X" Button Scale
    Vector3 xScaleBig = new Vector3(3.5f, 3.5f, 3.5f);  //"X" Button Scailing
    Vector3 bigCircle = new Vector3(6, 6, 6);           //"X" Button Circle Scale
    Vector3 enemyScale;
    Vector3 rhythmTextScale;
    
    public GameObject enemy;
    public EBeatScore beatScore;

    bool scaling;                   //Button scailing
    bool pulsing = false;           //Used to check determine beat check
    float timeBetweenBeats = 3.8f;  
    float flux = 1.32f;

    
    float beatTime = 0.0f;  //Button press time
    int scaleCount = 0;     
    int tempBeat = 0;
    int rhythmTextLeanId;


    // Start is called before the first frame update
    void Start()
    {
        rhythmTextScale = rhythmText.transform.localScale;
        enemyScale = enemy.transform.localScale;
        scaling = false;
        BeatMaster.Beat += BeatCheck;
        BeatMaster.Beat += BeatX;
    }

    // Update is called once per frame
    void Update()
    {
        if(pulsing && Input.GetButtonDown("Jump"))
        {
            //Determines player score on how long it took to press the button on beat
            if (beatTime <= 0.15f)
                beatScore = EBeatScore.perfect;

            if (beatTime > 0.15f && beatTime <= 0.3f)
                beatScore = EBeatScore.good;

            if (beatTime > 0.3f && beatTime <= 0.5f)
                beatScore = EBeatScore.ok;

            if (beatTime > 0.5f)
                beatScore = EBeatScore.missed;

            StartCoroutine(DestroyEnemy());
            LeanTween.alpha(enemy, 0, 6);
        }
        if (pulsing)
            beatTime += Time.deltaTime;
        else
            beatTime = 0;  
    }

    public void BeatCheck(int beat)
    {
        //Checks for beat 0
        if ((beat+3) % 4 == 0 && !pulsing && !scaling)
        {
            //Starts scaling outer "X" circle
            StartCoroutine(Scale());
        }

        if (scaling)
        {
            if(beat != tempBeat)
            {
                scaleCount++;
            }
            tempBeat = beat;
        }
        else
        {
            scaleCount = 0;
            tempBeat = 0;
        }
    }

    IEnumerator Scale()
    {
        //Scales "X" outer circle
        LeanTween.scale(xCircle.gameObject, bigCircle, 0.15f);
        //Set scaling true
        scaling = true;
        while (scaleCount < 9)
        {
            //"X" Outer Circle Scale
            xCircle.transform.localScale -= new Vector3(flux * Time.deltaTime, flux * Time.deltaTime, flux * Time.deltaTime);
            yield return null;
        }
        //X Button pulse
        StartCoroutine(XPulse());
        //Set pulsing true
        pulsing = true;
    }


    public void BeatX(int beat)
    {
        if(pulsing == true)
        StartCoroutine(XPulse());
    }

    IEnumerator XPulse()
    {
        pulsing = true;
        int id  = LeanTween.scale(xButton.gameObject, xScaleBig, 0.15f).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        xButton.transform.localScale = xScale;
    }

    IEnumerator DestroyEnemy()
    {
        //Sets beat score to text
        rhythmText.text = beatScore.ToString();
        //Enable Text
        rhythmText.gameObject.SetActive(true);

        ResetRhythmTween();
        rhythmTextLeanId = LeanTween.scale(rhythmText.gameObject, rhythmTextScale, .75f).setEaseOutElastic().id;
        int id = LeanTween.scale(enemy, xScale/1.5f, 0.9f).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        rhythmText.gameObject.SetActive(false);
        pulsing = false;
        scaling = false;
        enemy.transform.localScale = enemyScale;
        enemy.SetActive(false);
        smoothCamera.cameraPosition = SmoothCameraScript.ECameraPosition.Normal;
        smoothCamera.StartCoroutine(smoothCamera.CameraSwitch(2));
        gameObject.SetActive(false);
    }

    void ResetRhythmTween()
    {
        if (rhythmTextLeanId != 0)
            LeanTween.cancel(rhythmTextLeanId);
        rhythmText.transform.localScale = Vector3.one * 0.01f;
    }




}
