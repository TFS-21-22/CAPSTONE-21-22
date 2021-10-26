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

    [SerializeField] private Text rhythmText;                   //Prints beat score
    [SerializeField] private SmoothCameraScript smoothCamera;   //Camera Smoothing

    private Vector3 xScale = new Vector3(3, 3, 3);              //"X" Button Scale
    private Vector3 xScaleBig = new Vector3(3.5f, 3.5f, 3.5f);  //"X" Button Scailing
    private Vector3 bigCircle = new Vector3(6, 6, 6);           //"X" Button Circle Scale
    private Vector3 enemyScale;
    private Vector3 rhythmTextScale;

    [SerializeField] private GameObject[] buttonBG = new GameObject[4];
    [SerializeField] private GameObject[] keyDirection = new GameObject[4];
    public GameObject enemy;


    bool scaling;                   //Button scailing
    bool pulsing = false;           //Used to check determine beat check
    float timeBetweenBeats = 3.8f;  
    float flux = 1.45f;

    
    float beatTime = 0.0f;  //Button press time
    int scaleCount = 0;     
    int tempBeat = 0;
    int rhythmTextLeanId;

    float smoothSpeed = 1f;
    [SerializeField] private Transform buttonCenter;
    [SerializeField] private Transform[] keyPositions = new Transform[3];

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
        //enemyScale = enemy.transform.localScale;
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
            {
                RandomBackground(buttonBG, keyDirection, keyPositions, Random.Range(0, buttonBG.Length), Random.Range(0, buttonBG.Length), false, Random.Range(0, 3)); 
                //beatScore = EBeatScore.perfect;
            }

            if (beatTime > 2.5f && beatTime <= 3f)
            {
                RandomBackground(buttonBG, keyDirection, keyPositions, Random.Range(0, buttonBG.Length), Random.Range(0, buttonBG.Length), false, Random.Range(0, 3));
            }

            if (beatTime > 2.2f && beatTime <= 2.5f)
            {
                RandomBackground(buttonBG, keyDirection, keyPositions, Random.Range(0, buttonBG.Length), Random.Range(0, buttonBG.Length), false, Random.Range(0, 3));
            }

            if (beatTime < 2.2f || beatTime > 3.6f)
            {
                RandomBackground(buttonBG, keyDirection, keyPositions, Random.Range(0, buttonBG.Length), Random.Range(0, buttonBG.Length), false, Random.Range(0, 3));
            }


            //StartCoroutine(DestroyEnemy());
            StartCoroutine(DestroyBoss());
            //LeanTween.alpha(enemy, 0, 6);
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
            StartCoroutine(ScaleCircle());
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

    void RandomBackground(GameObject[] bg, GameObject[] key, Transform[] startPos, int randomBG, int randomDirection, bool active, int randomKey)
    {
        float count = 0f;

        if (active)
        {
            count += Time.deltaTime;
            bg[randomBG].gameObject.SetActive(true);
            GameObject arrow = key[randomKey] as GameObject;
            arrow.SetActive(true);
            arrow.transform.position = startPos[randomDirection].transform.position;

            while(count < 5)
            arrow.transform.position = Vector3.Lerp(transform.position, buttonCenter.transform.position, smoothSpeed * Time.deltaTime);
        }
        else
        {
            for(int i = 0; i < bg.Length; i++)
            {
                bg[i].gameObject.SetActive(false);
                key[i].gameObject.SetActive(false);
            }
        }
        


    }

    IEnumerator ScaleCircle()
    {
        pulsing = true;
        //Random UI BG + Random Key Direction
        RandomBackground(buttonBG, keyDirection, keyPositions, Random.Range(0, buttonBG.Length), Random.Range(0, buttonBG.Length), true, Random.Range(0, 3));

        //Scales "X" outer circle
        LeanTween.scale(xCircle.gameObject, bigCircle, 0.15f);

        //Set scaling true
        scaling = true;

        while (scaleCount < 9)
        {
            //Outer Circle Scale
            xCircle.transform.localScale -= new Vector3(flux * Time.deltaTime, flux * Time.deltaTime, flux * Time.deltaTime);
            yield return null;
        }

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
        //-------------
        //Camera
        smoothCamera.cameraPosition = SmoothCameraScript.ECameraPosition.Normal;
        smoothCamera.StartCoroutine(smoothCamera.CameraSwitch(2));
        //-------------
        gameObject.SetActive(false);
    }

    IEnumerator DestroyBoss()
    {
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
