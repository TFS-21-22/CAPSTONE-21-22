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
    public Animator tigerAnimator;

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

    [EventID]
    public string eventID;

    void OnEnable()
    {
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

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(beatTime);

        if (pulsing && Input.GetButtonDown("Jump"))
        {
           // print("SCAPE PRESSED");
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
        tigerScript.attacking = true;
        scaling = true;
        pulsing = true;
        xCircle.gameObject.SetActive(true);
        xCircle.transform.localScale = bigCircle;

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
            TigerDamaged(currentEnemyQTE, false);
        }

        if (beatTime >= 1.111 && beatTime < 1.474)
        {
            RandomBackgroundActive(buttonBG, false);
            StartCoroutine(ScoreTextResult(okay, 1f));                  //Display Score Result
            StartCoroutine(DestroyEnemyQTE(1f, okay, currentEnemyQTE));
            TigerDamaged(currentEnemyQTE, false);

        }

        if (beatTime >= 1.474 && beatTime <= 1.792)
        {
            tigerAnimator.SetTrigger("Hit");
            RandomBackgroundActive(buttonBG, false);
            StartCoroutine(ScoreTextResult(good, 1f));                  //Display Score Result
            StartCoroutine(DestroyEnemyQTE(1f, good, currentEnemyQTE));
            TigerDamaged(currentEnemyQTE, true);
        }
        //.173

        if (beatTime > 1.792 && beatTime < 2f)
        {
            tigerAnimator.SetTrigger("Hit");
            RandomBackgroundActive(buttonBG, false);
            StartCoroutine(ScoreTextResult(perfect, 1f));
            StartCoroutine(DestroyEnemyQTE(1f, perfect, currentEnemyQTE));
            TigerDamaged(currentEnemyQTE, true);
        }

        if (beatTime >= 2f)
        {
            RandomBackgroundActive(buttonBG, false);
            StartCoroutine(ScoreTextResult(miss, 1f));
            StartCoroutine(DestroyEnemyQTE(1f, miss, currentEnemyQTE));
            TigerDamaged(currentEnemyQTE, false);
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
            {
                _currentEnemy.SetActive(false);
                _currentEnemy = null;
            }
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
            StartCoroutine(tigerScript.MoveTiger());

            if (_damageTiger)
            {
                float damageAmount = 20f;
                tigerScript.currentHealth -= damageAmount;
            }
            else
            {
                //Remove health
                float damageAmount = 34f;
                GameManager.instance.health -= damageAmount;
            }
        }

        if(tigerScript.currentHealth <= 0)
        {
            strafeScript.tigerAlive = false;
            tiger.SetActive(false);
        }

        tigerScript.attacking = false;
    }
}