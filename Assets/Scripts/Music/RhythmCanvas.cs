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
    public PauseMenuManager pauseMenuScript;

    public Image xButton;   //"X" button image
    public Image xCircle;   //"X" image outer circle

    [SerializeField] private SmoothCameraScript smoothCamera;   //Camera Smoothing

    public GameObject currentEnemyQTE;

    private Vector3 xScale = new Vector3(3, 3, 3);              //"X" Button Scale
    private Vector3 xScaleBig = new Vector3(3.5f, 3.5f, 3.5f);  //"X" Button Scailing
    private Vector3 bigCircle = new Vector3(6, 6, 6);           //"X" Button Circle Scale
    private Vector3 enemyScale;
    private Vector3 rhythmTextScale;
    public Vector3 scoreTextStartPos;

    [SerializeField] private GameObject[] buttonBG = new GameObject[4];     //Button Backgrounds
    public GameObject wisp;

    //Score Text
    [SerializeField] private GameObject perfect;
    [SerializeField] private GameObject good;
    [SerializeField] private GameObject okay;
    [SerializeField] private GameObject miss;
    [SerializeField] private GameObject tiger;

    public bool scaling;                   //Button scailing
    public bool pulsing = false;           //Used to check determine beat check
    private float timeBetweenBeats = 3.8f;
    private float flux = 1.85f;

    private bool sequencePressed = false;
    int buttonBGarrayIndex;
    int buttonScoreTextarrayIndex;

    private double beatTime = 0.0f;  //Button press time
    private int scaleCount = 0;
    private int tempBeat = 0;
    private int rhythmTextLeanId;

    float smoothSpeed = 1f;

    [SerializeField] private Transform buttonCenter;
    [SerializeField] private Transform[] keyPositions = new Transform[3];

    private bool buttonPressed = false;

    float timeDebug;

    int bossButtonPresses = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreTextStartPos = perfect.transform.position;
        scaling = false;
        BeatMaster.Beat += BeatCheck;
        BeatMaster.Beat += BeatX;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(beatTime);

        if (pulsing && Input.GetButtonDown("Jump"))
        {

            buttonPressed = true;
            xCircle.gameObject.SetActive(false);
            //sequencePressed = true;
            if (beatTime < 1.284f && beatTime >= 0)
            {
                RandomBackground(buttonBG, false);
                StartCoroutine(ScoreTextResult(miss, 1f));                  //Display Score Result
                StartCoroutine(DestroyEnemyQTE(1f, miss, currentEnemyQTE)); //Enemy Destroy

            }

            if (beatTime >= 1.284f && beatTime < 1.647f)
            {
                RandomBackground(buttonBG, false);
                StartCoroutine(ScoreTextResult(okay, 1f));                  //Display Score Result
                StartCoroutine(DestroyEnemyQTE(1f, okay, currentEnemyQTE)); //Enemy Destroy

            }

            if (beatTime >= 1.647f && beatTime < 1.985f)
            {
                RandomBackground(buttonBG, false);
                StartCoroutine(ScoreTextResult(good, 1f));                  //Display Score Result
                StartCoroutine(DestroyEnemyQTE(1f, good, currentEnemyQTE)); //Enemy Destroy
            }

            if (beatTime > 1.985f && beatTime < 2.126f)
            {
                RandomBackground(buttonBG, false);
                StartCoroutine(ScoreTextResult(perfect, 1f));

                //Enemy Destroy
                StartCoroutine(DestroyEnemyQTE(1f, perfect, currentEnemyQTE));
            }
        }

        if (beatTime > 2.126f && !buttonPressed)
        {

            RandomBackground(buttonBG, false);
            StartCoroutine(ScoreTextResult(miss, 1f));                  //Display Score Result
            StartCoroutine(DestroyEnemyQTE(1f, miss, currentEnemyQTE)); //Enemy Destroy
        }

        if (pulsing)
        {
            if (!pauseMenuScript.isPaused)
            {
                beatTime += Time.deltaTime;
            }
        }
        else
            beatTime = 0;

    }

    public void BeatCheck(int beat)
    {
        //Checks for beat 0
        if ((beat + 3) % 4 == 0 && !pulsing && !scaling)
        {
            //Starts scaling outer "X" circle
            StartCoroutine(ScaleCircle());
        }

        if (scaling)
        {
            if (beat != tempBeat)
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

    public void RandomBackground(GameObject[] bg, bool setActive)
    {

        if (setActive)
        {
            buttonBGarrayIndex = Random.Range(0, buttonBG.Length);
            bg[buttonBGarrayIndex].gameObject.SetActive(true);
        }
        else
        {
            buttonBGarrayIndex = Random.Range(0, buttonBG.Length);
            bg[buttonBGarrayIndex].gameObject.SetActive(false);
        }

    }

    IEnumerator ScaleCircle()
    {
        ResetRhythmTween();
        scaling = true;
        pulsing = true;
        RandomBackground(buttonBG, true);//Random UI BG + Random Key Direction
        LeanTween.scale(xCircle.gameObject, bigCircle, 0.15f);                                                              //Scales "X" outer circle


        while (scaleCount < 9)
        {
            //Outer Circle Scale
            if (!pauseMenuScript.isPaused)
                xCircle.transform.localScale -= new Vector3(flux * Time.deltaTime, flux * Time.deltaTime, flux * Time.deltaTime);
            yield return null;
        }
    }

    public void BeatX(int beat)
    {
        if (sequencePressed)
            StartCoroutine(Pulsing());
    }

    IEnumerator Pulsing()
    {
        yield return null;
        xCircle.transform.localScale = xScale;
    }

    IEnumerator DestroyEnemyQTE(float wait, GameObject _scoreText, GameObject _currentEnemy)
    {
        //Enable Text
        ResetRhythmTween();
        int id = LeanTween.scale(wisp.gameObject, new Vector3(5f, 5f, 5f), 1f).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        //Camera
        smoothCamera.cameraPosition = SmoothCameraScript.ECameraPosition.Normal;
        smoothCamera.StartCoroutine(smoothCamera.CameraSwitch(3));
        pulsing = false;
        scaling = false;
        xCircle.gameObject.SetActive(true);
        _currentEnemy.SetActive(false);
        _currentEnemy = null;
        _scoreText.transform.localScale = new Vector3(1f, 1f, 1f);
        _scoreText.transform.position = scoreTextStartPos;
        if (_scoreText.activeSelf)
            _scoreText.SetActive(false);
        LeanTween.cancelAll(true);
        this.gameObject.SetActive(false);

    }

    public void ResetRhythmTween()
    {
        if (rhythmTextLeanId != 0)
            LeanTween.cancel(rhythmTextLeanId);
    }

    IEnumerator ScoreTextResult(GameObject scoreText, float wait)
    {
        if (!scoreText.activeSelf)
            scoreText.SetActive(true);

        ResetRhythmTween();
        int id = LeanTween.scale(scoreText, new Vector3(2f, 2f, 2f), 1f).id;
        int id2 = LeanTween.moveY(scoreText, 0.05f, 2f).id;
        

        while (LeanTween.isTweening(id))
        {
            yield return null;

        }

        while (LeanTween.isTweening(id2))
        {
            yield return null;
        }
    }

    IEnumerator ResetScoreTextResult(float _wait, GameObject _scoreText)
    {
        yield return new WaitForSecondsRealtime(_wait);
    }
}