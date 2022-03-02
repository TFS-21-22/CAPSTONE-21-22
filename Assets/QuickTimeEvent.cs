using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class QuickTimeEvent : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons = new GameObject[12];
    [SerializeField] private RectTransform startPos;
    private float beatTempo;
    private int activeButtonCount;
    private Queue<int> activeButtons = new Queue<int>();
    private KeyCode keyToPressOne, keyToPressTwo, keyToPressThree;
    Vector3 resultsScale = new Vector3(3, 3, 3);
    private double beatTimeOne, beatTimeTwo, beatTimeThree;
    private bool sequenceOneStart, sequenceTwoStart, sequenceThreeStart;
    private bool endQTE = false;

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
    }
    // Start is called before the first frame update
    void Start()
    {
        beatTempo = (BeatMaster.instance.BPM / 60);
        activeButtonCount = 0;
        beatTimeOne = 0;
        beatTimeTwo = 0;
        beatTimeThree = 0;
        imgStartPos = startPos.position;
    }

    void Update()
    {
        if (sequenceOneStart)
        {
            beatTimeOne += Time.time;
        }
        else
            beatTimeOne = 0;

        if (sequenceTwoStart)
        {
            beatTimeTwo += Time.time;
        }
        else
            beatTimeTwo = 0;


        if (sequenceThreeStart)
        {
            beatTimeThree += Time.time;
        }
        else
            beatTimeThree = 0;
    }

    private void ButtonSequence(int _beat)
    {
        if ((_beat + 3) % 4 == 0)
        {
            if(activeButtonCount < 3)
                GetButton();
        }
    }

    private void GetButton()
    {
        //Get Random Index
        int buttonIndex = Random.Range(0, buttons.Length);

        //Check the list to see if this button is already active
        if (!activeButtons.Contains(buttonIndex))
        {
            activeButtonCount++;
        }
        else
        {
            GetButton();
        }


        if (activeButtonCount == 1 && !activeButtons.Contains(buttonIndex) && !sequenceOneStart)
        {
            sequenceOneStart = true;
            //Key to press
            KeyCode key = GetKeyToPress(buttonIndex);
            //Store key to press
            keyToPressOne = key;
            //Store array index
            activeButtons.Enqueue(buttonIndex);
            //Start button one seqence
            StartCoroutine(ButtonOne(buttons[buttonIndex], buttonIndex));
        }

        if (activeButtonCount == 2 && !activeButtons.Contains(buttonIndex) && !sequenceTwoStart)
        {
            sequenceTwoStart = true;
            //Key to press
            KeyCode key = GetKeyToPress(buttonIndex);
            //Store key to press
            
            keyToPressTwo = key;
            //Store array index
            activeButtons.Enqueue(buttonIndex);
            //Start button one seqence
            StartCoroutine(ButtonTwo(buttons[buttonIndex], buttonIndex));
        }

        if (activeButtonCount == 3 && !activeButtons.Contains(buttonIndex) && !sequenceThreeStart)
        {
            sequenceThreeStart = true;
            //Key to press
            KeyCode key = GetKeyToPress(buttonIndex);
            //Store key to press
            
            keyToPressThree = key;
            //Store array index
            activeButtons.Enqueue(buttonIndex);
            //Start button one seqence
            StartCoroutine(ButtonThree(buttons[buttonIndex], buttonIndex));
        }
    }


    private IEnumerator ButtonOne(GameObject _currentButton, int _arrayIndex)
    {
        //Enable button
        _currentButton.SetActive(true);
        //Move
        RectTransform rect = _currentButton.gameObject.GetComponent<RectTransform>();
        int id = rect.LeanMoveLocalX(-1450f, beatTempo).id;
        while (LeanTween.isTweening(id))
        {
            bool input = Input.GetKeyDown(keyToPressOne);
            Debug.Log(keyToPressTwo.ToString() + " Pressed: " + input);
            if (input)
            {
                break;
            }
            yield return null;
        }
        double time = beatTimeOne;
        activeButtons.Dequeue();
        sequenceOneStart = false;
        //Reset
        LeanTween.cancel(id);
        rect.position = startPos.position;
        _currentButton.SetActive(false);
        //Check time
        if (time > 100)
        {
            StartCoroutine(DisplayResult(perfectResult));
        }
        else
        {
            StartCoroutine(DisplayResult(missResult));
        }
        

    }



    private IEnumerator ButtonTwo(GameObject _currentButton, int _arrayIndex)
    {
        _currentButton.SetActive(true);
        //Move
        RectTransform rect = _currentButton.gameObject.GetComponent<RectTransform>();
        int id = rect.LeanMoveLocalX(-1450f, beatTempo).id;
        while (LeanTween.isTweening(id))
        {
            bool input = Input.GetKeyDown(keyToPressTwo);
            if (input && !sequenceOneStart)
            {
                sequenceTwoStart = false;
                break;
            }
            yield return null;
        }
        double time = beatTimeOne;
        //Reset
        sequenceTwoStart = false;
        activeButtons.Dequeue();
        LeanTween.cancel(id);
        rect.position = startPos.position;
        _currentButton.SetActive(false);
        //Check time
        if (time > 100)
        {
            StartCoroutine(DisplayResult(perfectResult));
        }
        else
        {
            StartCoroutine(DisplayResult(missResult));
        }
        

    }

    private IEnumerator ButtonThree(GameObject _currentButton, int _arrayIndex)
    {
        _currentButton.SetActive(true);
        //Move
        RectTransform rect = _currentButton.gameObject.GetComponent<RectTransform>();
        int id = rect.LeanMoveLocalX(-1450f, beatTempo).id;
        while (LeanTween.isTweening(id))
        {
            bool input = Input.GetKeyDown(keyToPressThree);
            if (input && !sequenceTwoStart)
            {
                sequenceThreeStart = false;
                break;
            }
            yield return null;
        }
        double time = beatTimeOne;
        sequenceThreeStart = false;
        endQTE = true;
        //Reset
        activeButtons.Dequeue();
        LeanTween.cancel(id);
        rect.position = startPos.position;
        _currentButton.SetActive(false);
        //Check time
        if (time > 100)
        {
            StartCoroutine(DisplayResult(perfectResult));
        }
        else
        {
            StartCoroutine(DisplayResult(missResult));
        }
        

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
        //Reset result position
        _result.transform.localScale = new Vector3(1, 1, 1);
        //Disable result
        _result.SetActive(false);

        //QTE finished
        if (endQTE)
        {
            activeButtonCount = 0;
            sequenceOneStart = false;
            sequenceTwoStart = false;
            sequenceThreeStart = false;
            LeanTween.cancelAll();
            endQTE = false;
            this.gameObject.SetActive(false);
        }
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
