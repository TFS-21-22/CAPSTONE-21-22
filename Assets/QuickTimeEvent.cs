using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class QuickTimeEvent : MonoBehaviour
{
    public PauseMenuManager pauseMenuScript;
    public Animator anim;

    [SerializeField] private GameObject[] buttons = new GameObject[12];
    [SerializeField] private RectTransform startPos;
    private float beatTempo;
    private int activeButtonCount;
    private Queue<int> activeButtons = new Queue<int>();
    private KeyCode keyToPressOne, keyToPressTwo, keyToPressThree;
    Vector3 resultsScale = new Vector3(3, 3, 3);
    private double beatTimeOne, beatTimeTwo, beatTimeThree;
    private bool sequenceOneActive, sequenceTwoActive, sequenceThreeActive;
    private bool inputPressedOne, inputPressedTwo, inputPressedThree;
    private bool endQTE = false;
    private int poseCount = 0;

    [SerializeField] private GameObject perfectResult;
    [SerializeField] private GameObject missResult;

    

    Vector2 imgStartPos;

    void OnDisable()
    {
        BeatMaster.Beat -= ButtonSequence;
    }

    void OnEnable()
    {
        BeatMaster.Beat += ButtonSequence;
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
        //print("Active Buttons: " + activeButtonCount);
        //print("BeatTime 1: " + beatTimeOne);
        //print("BeatTime 2: " + beatTimeOne);
        //print("BeatTime 3: " + beatTimeOne);
        //print("sequenceOne: " + sequenceOneStart);
        //print("sequenceTwo: " + sequenceTwoStart);
        //print("sequenceThree: " + sequenceThreeStart);




    }
    // Start is called before the first frame update
    void Start()
    {
        beatTempo = (BeatMaster.instance.BPM / 60);
    }

    void Update()
    {
        //MARLON USE THE "POSECOUNT" VARIABLE TO DETERMINE WHAT POSE THEY ARE ON I ALREADY ADDED IT IN THE CODE JUST MAKE IT WORK WITH THE ANIMATOR
        //EVERYTIME THE PLAYER PRESSES GET A PERFECT IT WILL INCREMENT IF THEY DONT THE POSE COUNT WILL STAY THE SAME // FYI GIVE ANIM A REFERENCE ITS NULL
      
        if (sequenceOneActive)
            beatTimeOne += Time.deltaTime;
        else
            beatTimeOne = 0;


        if (sequenceTwoActive)
            beatTimeTwo += Time.deltaTime;
        else
            beatTimeTwo = 0;

        if (sequenceThreeActive)
            beatTimeThree += Time.deltaTime;
        else
            beatTimeThree = 0;

        
    }

    private void ButtonSequence(int _beat)
    {
        if ((_beat + 3) % 4 == 0)
        {
            if(activeButtonCount < 3)
            {
                GetButton();
            }
        }
    }

    private void GetButton()
    {
        print("GET BUTTON");
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

    //Not being used for now
    private IEnumerator DisplayButton(GameObject _currentButton, int _arrayIndex, bool _sequence, KeyCode _keyToPress)
    {
        bool inputPressed = false;
        _sequence = true;                                                               //Sequence start
        _currentButton.SetActive(true);                                                 //Enable button
        RectTransform rect = _currentButton.gameObject.GetComponent<RectTransform>();   //Get rect for button
        int id = rect.LeanMoveLocalX(-1450f, beatTempo).id;                             //Move button
        while (LeanTween.isTweening(id))
        {
            bool input = Input.GetKeyDown(_keyToPress);
            if (input)
            {
                if (sequenceOneActive)
                {
                    inputPressed = true;
                }
                if (sequenceTwoActive && !sequenceOneActive)
                {
                    inputPressed = true;
                }
                if (sequenceThreeActive && !sequenceTwoActive)
                {
                    inputPressed = true;
                }
                break;
            }
            yield return null;
        }

        if (inputPressed)
        {
            //DisplayHitOrMiss();//Check results
        }
        _sequence = false;                              //Disable sequence
        LeanTween.cancel(id);                           //Cancel leantween
        rect.transform.position = startPos.position;    //Reset button pos
        activeButtons.Dequeue();                        //Remove button from queue
        _currentButton.SetActive(false);                //Disable button
    }


    private IEnumerator ButtonOne(GameObject _currentButton, int _arrayIndex, KeyCode _keyToPress)
    {
        bool inputPressed = false;      //Input check
        sequenceOneActive = true;       //Enable sequence
        _currentButton.SetActive(true); //Enable button

        //Rect
        RectTransform rect = _currentButton.gameObject.GetComponent<RectTransform>();

        //LeanTween
        int id = rect.LeanMoveLocalX(-1450f, beatTempo).id;
        while (LeanTween.isTweening(id))
        {
            bool input = Input.GetKeyDown(_keyToPress);
            if (input)
            {
                inputPressed = true;
                break;
            }
            yield return null;
        }

        LeanTween.cancel(id);                           //Cancel leantween
        rect.transform.position = startPos.position;    //Reset button rect pos
        if (inputPressed)
        {
            poseCount++;
            DisplayResult();
        }

        if (activeButtons.Count > 0)
            activeButtons.Dequeue();        //Remove active button
        sequenceOneActive = false;          //Disable sequence
        _currentButton.SetActive(false);    //Disable button
    }

    private IEnumerator ButtonTwo(GameObject _currentButton, int _arrayIndex, KeyCode _keyToPress)
    {
        bool inputPressed = false;      //Input check
        sequenceTwoActive = true;       //Enable sequence
        _currentButton.SetActive(true); //Enable button

        //Rect
        RectTransform rect = _currentButton.gameObject.GetComponent<RectTransform>();

        //LeanTween
        int id = rect.LeanMoveLocalX(-1450f, beatTempo).id;
        while (LeanTween.isTweening(id))
        {
            bool input = Input.GetKeyDown(_keyToPress);
            if (input && !sequenceOneActive)
            {
                inputPressed = true;
                break;
            }
            yield return null;
        }

        LeanTween.cancel(id);                           //Cancel leantween
        rect.transform.position = startPos.position;    //Reset button rect pos
        if (inputPressed)
        {
            poseCount++;
            DisplayResult();
        }

        if (activeButtons.Count > 0)
            activeButtons.Dequeue();        //Remove active button
        sequenceTwoActive = false;          //Disable sequence
        _currentButton.SetActive(false);    //Disable button
    }

    private IEnumerator ButtonThree(GameObject _currentButton, int _arrayIndex, KeyCode _keyToPress)
    {
        bool inputPressed = false;      //Input check
        sequenceOneActive = true;       //Enable sequence
        _currentButton.SetActive(true); //Enable button

        //Rect
        RectTransform rect = _currentButton.gameObject.GetComponent<RectTransform>();

        //LeanTween
        int id = rect.LeanMoveLocalX(-1450f, beatTempo).id;
        while (LeanTween.isTweening(id))
        {
            bool input = Input.GetKeyDown(_keyToPress);
            if (input && !sequenceTwoActive)
            {
                inputPressed = true;
                break;
            }
            yield return null;
        }

        LeanTween.cancel(id);                           //Cancel leantween
        rect.transform.position = startPos.position;    //Reset button rect pos

        if (inputPressed)
        {
            poseCount++;
            DisplayResult();
        }
        else
        {
            EndSequence();
        }

        if (activeButtons.Count > 0)
            activeButtons.Dequeue();        //Remove active button
        sequenceOneActive = false;          //Disable sequence
        _currentButton.SetActive(false);    //Disable button
    }

    private void EndSequence()
    {
        if (activeButtonCount >= 3)
            StartCoroutine(EndQuickTimeEvent(1.7f));
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

        
    }

    public void DisplayResult()
    {
        if (beatTimeOne >= 0.9805 && beatTimeOne < 1.2000)
        {
            StartCoroutine(DisplayResult(perfectResult));
        }
        else
        {
            StartCoroutine(DisplayResult(missResult));
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
}
