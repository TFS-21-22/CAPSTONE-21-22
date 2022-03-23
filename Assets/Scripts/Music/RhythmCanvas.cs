using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SonicBloom.Koreo;

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
    public Tiger tigerScript;
    public PauseMenuManager pauseMenuScript;
    public Strafe strafeScript;

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
    //Score Text
    [SerializeField] private GameObject perfect;
    [SerializeField] private GameObject good;
    [SerializeField] private GameObject okay;
    [SerializeField] private GameObject miss;
    [SerializeField] private GameObject tiger;

    public bool scaling;                   //Button scailing
    public bool pulsing = false;           //Used to check determine beat check
    public float flux = 3.6924f;
    private bool sequencePressed = false;
    private double beatTime = 0.0f;  //Button press time
    private int rhythmTextLeanId;
    private bool keyPressed = false;

    [SerializeField] private Transform buttonCenter;
    [SerializeField] private Transform[] keyPositions = new Transform[3];

    public AudioSource[] danceAudio;

    [EventID]
    public string eventID;

    // Animators
    public Animator tigerAnim;
    public Animator playerAnim;

    public ScoreSystem scoreSystem;

    private float scoreMultiplier = 0;

    public bool QTEBool = false;

    public bool tigerStun = false;

    void OnEnable()
    {
        tigerAnim = GetComponent<Animator>();
        playerAnim = GetComponent<Animator>();

        pulsing = false;
        scaling = false;
        keyPressed = false;
        scoreTextStartPos = perfect.transform.position;
        scaling = false;
        beatTime = 0f;
        RandomBackgroundActive(buttonBG, true);//Random UI BG + Random Key Direction
                                               //Starts scaling outer "X" circle
        if (!scaling)
        {
            StartCoroutine(ScaleCircle());
        }
    }

    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(beatTime);

        if (pulsing && Input.GetButtonDown("Jump"))
        {
            print("SCAPE PRESSED");
            keyPressed = true;
            xCircle.gameObject.SetActive(false);
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

    public void RandomBackgroundActive(GameObject[] bg, bool setActive)
    {

        if (setActive)
        {
            int rand = Random.Range(0, buttonBG.Length);
            bg[rand].SetActive(true);
        }
        else
        {
            foreach (GameObject background in buttonBG)
            {
                if(background.activeSelf)
                background.SetActive(false);
            }
        }

    }

    IEnumerator ScaleCircle()
    {
        float perfectM = 3.0f;
        float goodM = 2.0f;
        float defaultScore = 10f;

        

        scaling = true;
        pulsing = true;
        xCircle.gameObject.SetActive(true);
        xCircle.transform.localScale = bigCircle;
                                                          //Scales "X" outer circle
        while (xCircle.transform.localScale.x > 0.5f)
        {
            var spaceButtonInput = Input.GetButtonDown("Jump");
            if (spaceButtonInput)
            {
                break;
            }
            //Outer Circle Scale
            if (!pauseMenuScript.isPaused)
                xCircle.transform.localScale -= new Vector3(flux * Time.deltaTime, flux * Time.deltaTime, flux * Time.deltaTime);
            yield return null;
        }
        
        if ( beatTime >= 0 && beatTime < 1.111 )
        {
            RandomBackgroundActive(buttonBG, false);
            StartCoroutine(ScoreTextResult(miss, 1f));                  //Display Score Result
            StartCoroutine(DestroyEnemyQTE(1f, miss, currentEnemyQTE));
            if (tiger.activeSelf)
            {
                TigerDamaged(currentEnemyQTE, false);
            }
            danceAudio[0].Play();
        }

        if (beatTime >= 1.111 && beatTime < 1.474)
        {
            RandomBackgroundActive(buttonBG, false);
            StartCoroutine(ScoreTextResult(okay, 1f));                  //Display Score Result
            StartCoroutine(DestroyEnemyQTE(1f, okay, currentEnemyQTE));
            if (tiger.activeSelf)
            {
                TigerDamaged(currentEnemyQTE, false);
            }

            QTEBool = true;

            danceAudio[1].Play();
        }

        if (beatTime >= 1.474 && beatTime <= 1.792)
        {
            RandomBackgroundActive(buttonBG, false);
            StartCoroutine(ScoreTextResult(good, 1f));                  //Display Score Result
            StartCoroutine(DestroyEnemyQTE(1f, good, currentEnemyQTE));
            if (tiger.activeSelf)
            {
                TigerDamaged(currentEnemyQTE, true);
            }

            QTEBool = true;
            print(QTEBool);
            scoreSystem.score += defaultScore * goodM;
            danceAudio[2].Play();
        }        

        if (beatTime > 1.792 && beatTime < 2f)
        {
            RandomBackgroundActive(buttonBG, false);
            StartCoroutine(ScoreTextResult(perfect, 1f));
            StartCoroutine(DestroyEnemyQTE(1f, perfect, currentEnemyQTE));
            if (tiger.activeSelf)
            {
                TigerDamaged(currentEnemyQTE, true);
            }

            scoreSystem.score += defaultScore * perfectM;

            QTEBool = true;

            danceAudio[3].Play();
        }

        if (beatTime >= 2f)
        {
            RandomBackgroundActive(buttonBG, false);
            StartCoroutine(ScoreTextResult(miss, 1f));
            StartCoroutine(DestroyEnemyQTE(1f, miss, currentEnemyQTE));
            if (tiger.activeSelf)
            {
                TigerDamaged(currentEnemyQTE, false);
            }
            QTEBool = false;
            //print(QTEBool);
            danceAudio[0].Play();
        }
    }

    IEnumerator Pulsing()
    {
        yield return null;
        xCircle.transform.localScale = xScale;
    }

    IEnumerator DestroyEnemyQTE(float _wait, GameObject _scoreText, GameObject _currentEnemy)
    {
        yield return new WaitForSeconds(_wait);
        //Enable Text
        ResetRhythmTween();
        //Camera
        smoothCamera.cameraPosition = SmoothCameraScript.ECameraPosition.Normal;
        smoothCamera.StartCoroutine(smoothCamera.CameraSwitch(3));
        xCircle.gameObject.SetActive(false); 

        

        if (_currentEnemy)
        {
            if(!_currentEnemy.CompareTag("Tiger"))
            _currentEnemy.SetActive(false);
            _currentEnemy = null;
        }

        if (_scoreText)
        {
            _scoreText.transform.localScale = new Vector3(1f, 1f, 1f);
            _scoreText.transform.position = scoreTextStartPos;
            _scoreText.SetActive(false);
        }
       
        LeanTween.cancelAll(true);
        keyPressed = false;
        pulsing = false;
        scaling = false;
        xCircle.transform.localScale = bigCircle;
        beatTime = 0f;
        
        this.gameObject.SetActive(false);

        QTEBool = false;

        tigerStun = false;

       // tigerAnim.ResetTrigger("Hit");
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
        
        while (LeanTween.isTweening(id))
        {
            yield return null;

        }
    }

    IEnumerator ResetScoreTextResult(float _wait, GameObject _scoreText)
    {
        yield return new WaitForSecondsRealtime(_wait);
    }

    public void TigerDamaged(GameObject _currentEnemy, bool _damageTiger)
    {

        if (_currentEnemy.gameObject.CompareTag("Tiger"))
        {
            if (_damageTiger)
            {
                tigerStun = true;
                float damageAmount = 50f;
                tigerScript.currentHealth -= damageAmount;
                if(tigerScript.currentHealth <= 0)
                {
                    strafeScript.GetComponent<Strafe>().tigerAlive = false;
                    tiger.SetActive(false);
                }
                

               // tigerAnim.SetTrigger("Hit");
                
            }
            else
            {
                //Remove health
                float damageAmount = 34f;
                GameManager.instance.health -= damageAmount;

            }

        }
        
        
    }
}