using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using SonicBloom.Koreo;


public class QuickTimeEvent : MonoBehaviour
{
    public PauseMenuManager pauseMenuScript;
    public Animator anim;

    [SerializeField] private GameObject[] buttons = new GameObject[12];
    [SerializeField] private RectTransform startPos;
    public float timeToFinish = 7f;
    public int activeButtonCount;
    private Queue<int> activeButtons = new Queue<int>();
    private KeyCode keyToPressOne, keyToPressTwo, keyToPressThree;
    Vector3 resultsScale = new Vector3(3, 3, 3);
    private double beatTimeOne, beatTimeTwo, beatTimeThree;
    private bool sequenceOneActive, sequenceTwoActive, sequenceThreeActive;
    private bool inputPressedOne, inputPressedTwo, inputPressedThree;
    private bool endQTE = false;
    private int poseCount = 0;

    private float scoreMultiplier = 0;

    public ScoreSystem scoreSystem;

    [SerializeField] private GameObject perfectResult;
    [SerializeField] private GameObject okayResult;
    [SerializeField] private GameObject missResult;
    [SerializeField] private WispSpawner wispSpawner;

    float Timecheck;

    KeyCode seqeuence1KeyCode;
    KeyCode seqeuence2KeyCode;
    KeyCode seqeuence3KeyCode;

    Vector2 imgStartPos;

    public bool correctButtonSwitch;

   

    void OnEnable()
    {
        activeButtonCount = 0;
        beatTimeOne = 0;
        beatTimeTwo = 0;
        beatTimeThree = 0;
        sequenceOneActive = false;
        sequenceTwoActive = false;
        sequenceThreeActive = false;
        inputPressedOne = false;
        inputPressedTwo = false;
        inputPressedThree = false;
        poseCount = 0;

        
    }
    // Start is called before the first frame update
    void Start()
    {
        timeToFinish = (BeatMaster.instance.BPM / 60);
    }

    void Update()
    {
        if (sequenceOneActive)
            beatTimeOne += Time.deltaTime;

        if (sequenceTwoActive)
            beatTimeTwo += Time.deltaTime;
       

        if (sequenceThreeActive)
            beatTimeThree += Time.deltaTime;

        anim.SetInteger("POSECOUNT", poseCount);
        
    }

    public void GetButton()
    {
        //Get Random Index
        int buttonIndex = Random.Range(0, buttons.Length);

        //Check the list to see if this button is already active
        if (!activeButtons.Contains(buttonIndex))
        {
            activeButtons.Enqueue(buttonIndex);
            activeButtonCount++;
        }
        else
        {
            GetButton();
        }

        //Button one
        if (activeButtonCount == 1)
        {
            //Key to press
            KeyCode key = GetKeyToPress(buttonIndex);
            StartCoroutine(ButtonOne(buttons[buttonIndex], buttonIndex, key));
        }
        //button two
        if (activeButtonCount == 2)
        {
            //Key to press
            KeyCode key = GetKeyToPress(buttonIndex);
            StartCoroutine(ButtonTwo(buttons[buttonIndex], buttonIndex, key));
        }

        //Button three
        if (activeButtonCount == 3)
        {
            //Key to press
            KeyCode key = GetKeyToPress(buttonIndex);
            StartCoroutine(ButtonThree(buttons[buttonIndex], buttonIndex, key));
        }
    }

    private IEnumerator ButtonOne(GameObject _currentButton, int _arrayIndex, KeyCode _keyToPress)
    {
        seqeuence1KeyCode = _keyToPress;
        bool inputPressed = false;      //Input check
        sequenceOneActive = true;       //Enable sequence
        _currentButton.SetActive(true); //Enable button      
        wispSpawner.Spawn(_keyToPress); //Spawn Wisp

        //Rect
        RectTransform rect = _currentButton.gameObject.GetComponent<RectTransform>();

        //LeanTween
        int id = rect.LeanMoveLocalX(-1450f, 1.85f).id;
        while (LeanTween.isTweening(id))
        {

            bool input = Input.GetKeyDown(_keyToPress);
            if (input)
            {
                print(beatTimeOne);
                PoseCountCheck(_keyToPress);
                inputPressed = true;
                break;
            }
            yield return null;
        }

        LeanTween.cancel(id);                           //Cancel leantween
        rect.transform.position = startPos.position;    //Reset button rect pos
        if (inputPressed)
        {
            DisplayResult();
        }
      
   

        if (activeButtons.Count > 0)
            activeButtons.Dequeue();        //Remove active button
        sequenceOneActive = false;          //Disable sequence
        _currentButton.SetActive(false);    //Disable button
    }

    private IEnumerator ButtonTwo(GameObject _currentButton, int _arrayIndex, KeyCode _keyToPress)
    {
        seqeuence2KeyCode = _keyToPress;
        bool inputPressed = false;      //Input check
        sequenceTwoActive = true;       //Enable sequence
        _currentButton.SetActive(true); //Enable button
        wispSpawner.Spawn(_keyToPress); //Spawn Wisp

        //Rect
        RectTransform rect = _currentButton.gameObject.GetComponent<RectTransform>();

        //LeanTween
        int id = rect.LeanMoveLocalX(-1450f, 1.85f).id;
        while (LeanTween.isTweening(id))
        {
            bool input = Input.GetKeyDown(_keyToPress);
            if (input && !sequenceOneActive)
            {
                //print(beatTimeTwo);
                PoseCountCheck(_keyToPress);
                inputPressed = true;
                break;
            }
            yield return null;
        }

        LeanTween.cancel(id);                           //Cancel leantween
        rect.transform.position = startPos.position;    //Reset button rect pos
        if (inputPressed)
        {
           // poseCount++;
            DisplayResult();
        }

        if (activeButtons.Count > 0)
            activeButtons.Dequeue();        //Remove active button
        sequenceTwoActive = false;          //Disable sequence
        _currentButton.SetActive(false);    //Disable button
    }

    private IEnumerator ButtonThree(GameObject _currentButton, int _arrayIndex, KeyCode _keyToPress)
    {
        seqeuence3KeyCode = _keyToPress;
        bool inputPressed = false;      //Input check
        sequenceThreeActive = true;       //Enable sequence
        _currentButton.SetActive(true); //Enable button
        wispSpawner.Spawn(_keyToPress); //Spawn Wisp

        //Rect
        RectTransform rect = _currentButton.gameObject.GetComponent<RectTransform>();

        //LeanTween
        int id = rect.LeanMoveLocalX(-1450f, 1.85f).id;
        while (LeanTween.isTweening(id))
        {
            bool input = Input.GetKeyDown(_keyToPress);
            if (input && !sequenceTwoActive)
            {
                print(beatTimeThree);
                PoseCountCheck(_keyToPress);
                inputPressed = true;
                break;
            }
            yield return null;
        }

        LeanTween.cancel(id);                           //Cancel leantween
        rect.transform.position = startPos.position;    //Reset button rect pos

        if (inputPressed)
        {
            //poseCount++;
            DisplayResult();
        }
        else
        {
            EndSequence();
        }

        if (activeButtons.Count > 0)
            activeButtons.Dequeue();        //Remove active button
        sequenceThreeActive = false;          //Disable sequence
        _currentButton.SetActive(false);    //Disable button
    }

    private void EndSequence()
    {
        if (activeButtonCount >= 3)
            StartCoroutine(EndQuickTimeEvent(1.7f));
        StartCoroutine(PoseCountReset());

    }

    private IEnumerator DisplayResult(GameObject _result)
    {
        //Enable result
        _result.SetActive(true);
        //Scale result
        int id = LeanTween.scale(_result.gameObject, resultsScale, 0.5f).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        LeanTween.cancel(id);
        //Reset result position
        _result.transform.localScale = new Vector3(1, 1, 1);
        //Disable result
        _result.SetActive(false);

        //DESTROY WISP HERE
        correctButtonSwitch = false;
    }

    public void DisplayResult()
    {
        double perfectMinValue = 1.01917020976543;
        double perfectMaxValue = 1.11998789012432;

        double minValue = 0.825594299286604;
        double maxValue = 1.21341879799962;
        if (sequenceOneActive)
        {
            

            if (beatTimeOne >= minValue && beatTimeOne <= maxValue)
            {
                if(beatTimeOne >= perfectMinValue && beatTimeOne <= perfectMaxValue)
                {
                    StartCoroutine(DisplayResult(perfectResult));
                   
                   // poseCount++;

                    scoreMultiplier = 3;

                    scoreSystem.score += 100 * scoreMultiplier;

                    //DESTROY WISP HERE
                    correctButtonSwitch = true;
                }
                else
                {
                    StartCoroutine(DisplayResult(okayResult));
                   
                   // poseCount++;

                    scoreMultiplier = 2;

                    scoreSystem.score += 100 * scoreMultiplier;

                    //DESTROY WISP HERE
                    correctButtonSwitch = true;
                }
            }
            else
            {
                StartCoroutine(DisplayResult(missResult));
              //  poseCount--;

                scoreMultiplier = 0;

                scoreSystem.score += 100 * scoreMultiplier;
            }

            
        }

        if (sequenceTwoActive)
        {
            if (beatTimeTwo >= minValue && beatTimeTwo <= maxValue)
            {
                if (beatTimeTwo >= perfectMinValue && beatTimeTwo <= perfectMaxValue)
                {
                    StartCoroutine(DisplayResult(perfectResult));
                    
                    //poseCount++;

                    scoreMultiplier = 3;

                    scoreSystem.score += 100 * scoreMultiplier;

                    //DESTROY WISP HERE
                    correctButtonSwitch = true;
                }
                else
                {
                    StartCoroutine(DisplayResult(okayResult));
                    
                   // poseCount++;

                    scoreMultiplier = 2;

                    scoreSystem.score += 100 * scoreMultiplier;

                    //DESTROY WISP HERE
                    correctButtonSwitch = true;
                }
            }
            else
            {
                StartCoroutine(DisplayResult(missResult));
                
               // poseCount--;

                scoreMultiplier = 0;

                scoreSystem.score += 100 * scoreMultiplier;
            }

        }

        if (sequenceThreeActive)
        {
            if (beatTimeThree >= minValue && beatTimeThree < maxValue)
            {
                if (beatTimeThree >= perfectMinValue && beatTimeThree <= perfectMaxValue)
                {
                    StartCoroutine(DisplayResult(perfectResult));
                    
                    //poseCount++;

                    scoreMultiplier = 3;

                    scoreSystem.score += 100 * scoreMultiplier;

                    //DESTROY WISP HERE
                    correctButtonSwitch = true;
                }
                else
                {
                    StartCoroutine(DisplayResult(okayResult));
                   
                   // poseCount++;

                    scoreMultiplier = 2;

                    scoreSystem.score += 100 * scoreMultiplier;

                    //DESTROY WISP HERE
                    correctButtonSwitch = true;
                }
            }
            else
            {
                StartCoroutine(DisplayResult(missResult));

               // poseCount--;

                scoreMultiplier = 0;

                scoreSystem.score += 100 * scoreMultiplier;
            }
            
        }




        if (activeButtonCount >= 3)
        {
            EndSequence();            
            
        }
    }

    private IEnumerator EndQuickTimeEvent(float _wait)
    {
        yield return new WaitForSecondsRealtime(_wait);
        this.gameObject.SetActive(false);
    }

    public KeyCode GetKeyToPress(int _buttonIndex)
    {
        KeyCode leftArrow = KeyCode.LeftArrow;
        KeyCode rightArrow = KeyCode.RightArrow;
        KeyCode upArrow = KeyCode.UpArrow;
        KeyCode downArrow = KeyCode.DownArrow;

        if (_buttonIndex >= 0 && _buttonIndex <= 2)
        {
            return upArrow;
        }
        else if (_buttonIndex >= 3 && _buttonIndex <= 5)
        {
            return leftArrow;
        }
        else if (_buttonIndex >= 6 && _buttonIndex <= 8)
        {
            return rightArrow;
        }
        else
        {
            return downArrow;
        }
    }

    IEnumerator PoseCountReset()
    {
        yield return new WaitForSeconds(1.0f);

        if (poseCount > 0)
        {
            poseCount = 0;
        }

    }
    void PoseCountCheck(KeyCode currentKey)
    {
        KeyCode leftArrow = KeyCode.LeftArrow;
        KeyCode rightArrow = KeyCode.RightArrow;
        KeyCode upArrow = KeyCode.UpArrow;
        KeyCode downArrow = KeyCode.DownArrow;

        if (currentKey == rightArrow)
        {
            poseCount = 1;
            StartCoroutine(PoseCountReset());
        }

        if (currentKey == leftArrow)
        {
            poseCount = 2;
            StartCoroutine(PoseCountReset());
        }

        if (currentKey == upArrow)
        {
            poseCount = 3;
            StartCoroutine(PoseCountReset());
        }

        if(currentKey == downArrow)
        {
            poseCount = 4;
            StartCoroutine(PoseCountReset());
        }
    }
}
