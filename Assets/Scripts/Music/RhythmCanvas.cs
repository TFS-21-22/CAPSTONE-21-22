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
    public static RhythmCanvas instance;

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
    float flux = 1.45f;

    
    float beatTime = 0.0f;  //Button press time
    int scaleCount = 0;     
    int tempBeat = 0;
    int rhythmTextLeanId;

    float timeDebug;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance != null)
        {
            Destroy(this);
        }
    }

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
        if (pulsing && Input.GetButtonDown("Jump"))
        {
            if (beatTime >= 3f && beatTime <= 3.5f)
                beatScore = EBeatScore.perfect;

            if (beatTime > 2.5f && beatTime <= 3f)
                beatScore = EBeatScore.good;

            if (beatTime > 2.2f && beatTime <= 2.5f)
                beatScore = EBeatScore.ok;

            if (beatTime < 2.2f || beatTime > 3.6f)
                beatScore = EBeatScore.missed;

            //StartCoroutine(DestroyEnemy());
            StartCoroutine(DestroyBoss());
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
        pulsing = true;
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
        //StartCoroutine(XPulse());
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
        yield return null;
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

    IEnumerator DestroyBoss()
    {
        //Sets beat score to text
        rhythmText.text = beatScore.ToString();
        //Enable Text
        rhythmText.gameObject.SetActive(true);
        ResetRhythmTween();
        rhythmTextLeanId = LeanTween.scale(rhythmText.gameObject, rhythmTextScale, .75f).setEaseOutElastic().id;
        int id = LeanTween.scale(Tiger.instance.gameObject, xScale / 2f, 0.9f).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        rhythmText.gameObject.SetActive(false);
        pulsing = false;
        scaling = false;
        //Tiger.instance.gameObject.transform.localScale = enemyScale;
        Tiger.instance.gameObject.SetActive(false);
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
