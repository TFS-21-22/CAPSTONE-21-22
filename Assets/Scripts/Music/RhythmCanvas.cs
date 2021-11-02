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

    [SerializeField] private SmoothCameraScript smoothCamera;   //Camera Smoothing

    private Vector3 xScale = new Vector3(3, 3, 3);              //"X" Button Scale
    private Vector3 xScaleBig = new Vector3(3.5f, 3.5f, 3.5f);  //"X" Button Scailing
    private Vector3 bigCircle = new Vector3(6, 6, 6);           //"X" Button Circle Scale
    private Vector3 enemyScale;
    private Vector3 rhythmTextScale;

    [SerializeField] private GameObject[] buttonBG = new GameObject[4];     //Button Backgrounds
    [SerializeField] private GameObject[] keyDirection = new GameObject[4]; //Arrow Keys
    public GameObject enemy;

    //Score Text
    [SerializeField] private GameObject perfect;
    [SerializeField] private GameObject good;
    [SerializeField] private GameObject okay;
    [SerializeField] private GameObject miss;

    private bool scaling;                   //Button scailing
    private bool pulsing = false;           //Used to check determine beat check
    private float timeBetweenBeats = 3.8f;
    private float flux = 1.85f;

    private bool sequencePressed = false;
    int arrayIndex;

    private double beatTime = 0.0f;  //Button press time
    private int scaleCount = 0;
    private int tempBeat = 0;
    private int rhythmTextLeanId;

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
        scaling = false;
        BeatMaster.Beat += BeatCheck;
        BeatMaster.Beat += BeatX;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (pulsing && Input.GetButtonDown("Jump"))
        {
            sequencePressed = true;

            if ( beatTime < 1.85f && beatTime >= 0)
            {
                RandomBackground(buttonBG, keyDirection, Random.Range(0, buttonBG.Length), Random.Range(0, buttonBG.Length));
                StartCoroutine(ScoreTextResult(miss, 1f));

                if (Tiger.instance.gameObject.activeSelf)
                    StartCoroutine(DestroyBoss(1f));
            }

            if (beatTime > 1.84f && beatTime < 1.85f)
            {
                RandomBackground(buttonBG, keyDirection, Random.Range(0, buttonBG.Length), Random.Range(0, buttonBG.Length));
                StartCoroutine(ScoreTextResult(okay, 1f));

                if (Tiger.instance.gameObject.activeSelf)
                    StartCoroutine(DestroyBoss(1f));
            }

            if (beatTime >= 1.99f && beatTime < 2f)
            {
                RandomBackground(buttonBG, keyDirection, Random.Range(0, buttonBG.Length), Random.Range(0, buttonBG.Length));
                StartCoroutine(ScoreTextResult(good, 1f));

                if (Tiger.instance.gameObject.activeSelf)
                    StartCoroutine(DestroyBoss(1f));

            }

            if (beatTime > 2.2f && beatTime <= 2.3f)
            {
                RandomBackground(buttonBG, keyDirection, Random.Range(0, buttonBG.Length), Random.Range(0, buttonBG.Length));
                StartCoroutine(ScoreTextResult(perfect, 1f));

                if (Tiger.instance.gameObject.activeSelf)
                    StartCoroutine(DestroyBoss(1f));
            }

            if(beatTime > 3)
            {
                RandomBackground(buttonBG, keyDirection, Random.Range(0, buttonBG.Length), Random.Range(0, buttonBG.Length));
                StartCoroutine(ScoreTextResult(miss, 1f));

                if (Tiger.instance.gameObject.activeSelf)
                    StartCoroutine(DestroyBoss(1f));
            }
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

    public void RandomBackground(GameObject[] bg, GameObject[] key, int random, int randomDirection)
    {
        
        if (!sequencePressed)
        {
            Debug.Log("Active");
            arrayIndex = random;
            bg[random].gameObject.SetActive(true);
            key[random].gameObject.SetActive(true);
            
        }

        if (sequencePressed)
        {
            Debug.Log("In-Active");
            bg[arrayIndex].gameObject.SetActive(false);
            key[arrayIndex].gameObject.SetActive(false);
            //RhythmCanvas.instance.gameObject.SetActive(false);
        }
    }

    IEnumerator ScaleCircle()
    {
        scaling = true;
        pulsing = true;
        RandomBackground(buttonBG, keyDirection, Random.Range(0, buttonBG.Length), Random.Range(0, buttonBG.Length));//Random UI BG + Random Key Direction
        LeanTween.scale(xCircle.gameObject, bigCircle, 0.15f);                                                              //Scales "X" outer circle
        

        while (scaleCount < 9)
        {
            //Outer Circle Scale
            xCircle.transform.localScale -= new Vector3(flux * Time.deltaTime, flux * Time.deltaTime, flux * Time.deltaTime);
            yield return null;
        }

        
    }


    public void BeatX(int beat)
    {
        if(sequencePressed)
        StartCoroutine(Pulsing());
    }

    IEnumerator Pulsing()
    {
        yield return null;
        xCircle.transform.localScale = xScale;
    }

    IEnumerator DestroyEnemy()
    {
        //Enable Text
        ResetRhythmTween();
        int id = LeanTween.scale(enemy, xScale/1.5f, 0.9f).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        pulsing = false;
        scaling = false;
        enemy.transform.localScale = enemyScale;
        enemy.SetActive(false);
        //Camera
        smoothCamera.cameraPosition = SmoothCameraScript.ECameraPosition.Normal;
        smoothCamera.StartCoroutine(smoothCamera.CameraSwitch(2));
        gameObject.SetActive(false);
    }

    IEnumerator DestroyBoss(float wait)
    {
        ResetRhythmTween();
        int id = LeanTween.scale(Tiger.instance.gameObject, new Vector3(5f, 5f, 5f), 1f).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        pulsing = false;
        scaling = false;

        Tiger.instance.gameObject.transform.localScale = new Vector3(1, 1, 1);
        Tiger.instance.gameObject.SetActive(false);
        smoothCamera.cameraPosition = SmoothCameraScript.ECameraPosition.Normal;
        smoothCamera.StartCoroutine(smoothCamera.CameraSwitch(2));
        gameObject.SetActive(false);
    }

    public void ResetRhythmTween()
    {
        if (rhythmTextLeanId != 0)
            LeanTween.cancel(rhythmTextLeanId);
    }

    IEnumerator ScoreTextResult(GameObject scoreText, float wait)
    {
        if(!scoreText.activeSelf)
        scoreText.SetActive(true);

        ResetRhythmTween();
        int id = LeanTween.scale(scoreText, new Vector3(2f, 2f, 2f), 1f).id;
        int id2 = LeanTween.moveY(scoreText, 0.05f, 5f).id;

        while (LeanTween.isTweening(id))
        {
            yield return null;

        }

        while (LeanTween.isTweening(id2))
        {
            yield return null;
        }

        if (scoreText.activeSelf)
        scoreText.SetActive(false);
    }

    

}
