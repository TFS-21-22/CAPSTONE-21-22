using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class QuickTimeEvent : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons = new GameObject[12];
    [SerializeField] private RectTransform[] buttonStartPos = new RectTransform[12];
    private float beatTempo;
    private int activeButtonCount;
    private Queue<int> activeButtons = new Queue<int>();
    private KeyCode keyToPressOne, keyToPressTwo, keyToPressThree;
    Vector3 resultsScale = new Vector3(3, 3, 3);
    private double beatTimeOne, beatTimeTwo, beatTimeThree;
    private bool sequenceOneStart, sequenceTwoStart, sequenceThreeStart;

    [SerializeField] private GameObject perfectResult;
    [SerializeField] private GameObject missResult;



    // Start is called before the first frame update
    void Start()
    {
        BeatMaster.Beat += ButtonSequence;
        beatTempo = (BeatMaster.instance.BPM / 60);
        activeButtonCount = -1;
        beatTimeOne = 0;
        beatTimeTwo = 0;
        beatTimeThree = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform rectPosition = buttons[i].GetComponent<RectTransform>();
            buttonStartPos[i] = rectPosition;
        }
    }

    void Update()
    {
        if (sequenceOneStart)
        {

            beatTimeOne += Time.time;
            bool buttonOneInput = Input.GetKeyDown(keyToPressOne);
            if (buttonOneInput)
            {
                sequenceOneStart = false;
                if (beatTimeOne <= 152.25 && beatTimeOne >= 147.255)
                    StartCoroutine(DisplayResult(perfectResult, false));
                else
                    StartCoroutine(DisplayResult(missResult, false));
            }

            if (beatTimeOne > 152.25)
            {
                StartCoroutine(DisplayResult(missResult, false));
                sequenceOneStart = false;
            }
        }
        else
            beatTimeOne = 0;

        if (sequenceTwoStart)
        {
            beatTimeTwo += Time.time;
            bool buttonTwoInput = Input.GetKeyDown(keyToPressTwo);

            if (buttonTwoInput && !sequenceOneStart)
            {
                sequenceTwoStart = false;

                if (beatTimeTwo <= 152.25 && beatTimeTwo >= 147.255)
                    StartCoroutine(DisplayResult(perfectResult, false));
                else
                    StartCoroutine(DisplayResult(missResult, false));
            }

            if (beatTimeTwo > 152.25)
            {
                StartCoroutine(DisplayResult(missResult, false));
                sequenceTwoStart = false;
            }
        }
        else
            beatTimeTwo = 0;


        if (sequenceThreeStart && !sequenceTwoStart)
        {
            beatTimeThree += Time.time;
            bool buttonThreeInput = Input.GetKeyDown(keyToPressThree);

            if (buttonThreeInput)
            {
                sequenceThreeStart = false;

                if (beatTimeThree <= 152.25 && beatTimeThree >= 147.255)
                    StartCoroutine(DisplayResult(perfectResult, true));
                else
                    StartCoroutine(DisplayResult(missResult, true));
            }

            if (beatTimeThree > 152.25)
            {
                StartCoroutine(DisplayResult(missResult, true));
                sequenceThreeStart = false;
            }
        }
        else
            beatTimeThree = 0;
    }

    private void ButtonSequence(int _beat)
    {
        if ((_beat + 3) % 4 == 0)
        {
            GetButton();
        }
    }

    private void GetButton()
    {

        //Get Random Index
        int buttonIndex = Random.Range(0, buttons.Length);

        //Check the list to see if this button is already active
        if (activeButtons.Contains(buttonIndex))
            GetButton();
        else
            activeButtonCount++;

        if (activeButtonCount == 0)
        {
            //Key to press
            KeyCode key = GetKeyToPress(buttonIndex);
            //Store key to press
            keyToPressOne = key;
            //Store array index
            activeButtons.Enqueue(buttonIndex);
            //Start button one seqence
            StartCoroutine(ButtonOne(buttons[buttonIndex], key, buttonIndex));
        }

        if (activeButtonCount == 1)
        {
            //Key to press
            KeyCode key = GetKeyToPress(buttonIndex);
            //Store key to press
            keyToPressTwo = key;
            //Store array index
            activeButtons.Enqueue(buttonIndex);
            //Start button one seqence
            StartCoroutine(ButtonTwo(buttons[buttonIndex], key, buttonIndex));
        }

        if (activeButtonCount == 2)
        {
            //Key to press
            KeyCode key = GetKeyToPress(buttonIndex);
            //Store key to press
            keyToPressThree = key;
            //Store array index
            activeButtons.Enqueue(buttonIndex);
            //Start button one seqence
            StartCoroutine(ButtonThree(buttons[buttonIndex], key, buttonIndex));
        }
    }


    private IEnumerator ButtonOne(GameObject _currentButton, KeyCode _keyToPress, int _arrayIndex)
    {

        sequenceOneStart = true;
        //Input
        bool input = Input.GetKeyDown(_keyToPress);
        //Enable button
        _currentButton.SetActive(true);

        //Move button to the left
        int id = LeanTween.moveLocalX(_currentButton.gameObject, -1450f, beatTempo).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }

        //Remove button as active button
        activeButtons.Dequeue();
        //Cancel leantween 
        LeanTween.cancel(_currentButton.gameObject, true);
        //Reset position
        RectTransform rectPos = _currentButton.GetComponent<RectTransform>();
        rectPos = buttonStartPos[_arrayIndex];
        //Disable button
        _currentButton.SetActive(false);

    }



    private IEnumerator ButtonTwo(GameObject _currentButton, KeyCode _keyToPress, int _arrayIndex)
    {
        sequenceTwoStart = true;
        //Input
        bool input = Input.GetKeyDown(_keyToPress);
        //Enable button
        _currentButton.SetActive(true);
        //Move the button to the left
        int id = LeanTween.moveLocalX(_currentButton.gameObject, -1450f, beatTempo).id;
        while (LeanTween.isTweening(id))
        {
            yield return null;
        }

        //Remove button as active button
        activeButtons.Dequeue();
        //Cancel leantween
        LeanTween.cancel(_currentButton.gameObject, true);
        //Reset button position
        RectTransform rectPos = _currentButton.GetComponent<RectTransform>();
        rectPos = buttonStartPos[_arrayIndex];
        //Disable button
        _currentButton.SetActive(false);

    }

    private IEnumerator ButtonThree(GameObject _currentButton, KeyCode _keyToPress, int _arrayIndex)
    {
        sequenceThreeStart = true;
        //Input
        bool input = Input.GetKeyDown(_keyToPress);
        //Enable button
        _currentButton.SetActive(true);
        //Move the button to the left
        int id = LeanTween.moveLocalX(_currentButton.gameObject, -1450f, beatTempo).id;
        while (LeanTween.isTweening(id))
        {

            yield return null;
        }
        //Remove button as active button
        activeButtons.Dequeue();
        //Cancel leantween
        LeanTween.cancel(_currentButton.gameObject, true);
        //Reset button position
        RectTransform rectPos = _currentButton.GetComponent<RectTransform>();
        rectPos = buttonStartPos[_arrayIndex];
        //Disable button
        _currentButton.SetActive(false);
    }

    private IEnumerator DisplayResult(GameObject _result, bool _lastSequence)
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
        if (_lastSequence)
        {
            this.gameObject.SetActive(false);
        }
    }

    public KeyCode GetKeyToPress(int _buttonIndex)
    {
        KeyCode leftArrow = KeyCode.LeftArrow;
        KeyCode rightArrow = KeyCode.RightArrow;
        KeyCode upArrow = KeyCode.UpArrow;
        KeyCode downArrow = KeyCode.DownArrow;

        if (_buttonIndex >= 0 && _buttonIndex <= 3)
        {
            return upArrow;
        }
        else if (_buttonIndex >= 4 && _buttonIndex <= 6)
        {
            return leftArrow;
        }
        else if (_buttonIndex >= 7 && _buttonIndex <= 9)
        {
            return rightArrow;
        }
        else
        {
            return downArrow;
        }
    }
}
