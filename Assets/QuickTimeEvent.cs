using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class QuickTimeEvent : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons = new GameObject[12];
    private float beatTempo;
    private int activeButtonCount;
    private Queue<int> activeButtons = new Queue<int>();
    private KeyCode keyToPress;


   

    // Start is called before the first frame update
    void Start()
    {
        BeatMaster.Beat += ButtonSequence;
        beatTempo = (BeatMaster.instance.BPM / 60);
        activeButtonCount = -1;
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
        print("Get Button");

        //Get Random Index
        int buttonIndex = Random.Range(0, buttons.Length);

        //Check the list if this button is already active
        if(activeButtons.Contains(buttonIndex))
        {
            GetButton();
            print("Get Button Again");

        }
        else
        {
            activeButtonCount++;
            print("Index Chosen");

        }

        KeyCode key = GetKeyToPress(buttonIndex);

        if (activeButtonCount == 0)
        {
            activeButtons.Enqueue(buttonIndex);
            StartCoroutine(ButtonOne(buttons[buttonIndex], key));
        }

        if (activeButtonCount == 1)
        {
            activeButtons.Enqueue(buttonIndex);
            StartCoroutine(ButtonTwo(buttons[buttonIndex], key));
        }

        if (activeButtonCount == 2)
        {
            activeButtons.Enqueue(buttonIndex);
            StartCoroutine(ButtonThree(buttons[buttonIndex], key));
        }
    }

   
    private IEnumerator ButtonOne(GameObject _currentButton, KeyCode _keyToPress)
    {
        //double time = Time.time;
        //var input = Input.GetKeyDown(_keyToPress);

        _currentButton.SetActive(true);

        int id = LeanTween.moveLocalX(_currentButton.gameObject, -1450f, beatTempo).id;

        while (LeanTween.isTweening(id))
        {
            yield return null;
        }
        activeButtons.Dequeue();
        _currentButton.SetActive(false);

    }

    

    private IEnumerator ButtonTwo(GameObject _currentButton, KeyCode _keyToPress)
    {
        _currentButton.SetActive(true);

        int id = LeanTween.moveLocalX(_currentButton.gameObject, -1450f, beatTempo).id;

        while (LeanTween.isTweening(id))
        {
            yield return null;
        }

        activeButtons.Dequeue();
        _currentButton.SetActive(false);

    }

    private IEnumerator ButtonThree(GameObject _currentButton, KeyCode _keyToPress)
    {
        _currentButton.SetActive(true);

        int id = LeanTween.moveLocalX(_currentButton.gameObject, -1450f, beatTempo).id;

        while (LeanTween.isTweening(id))
        {
            yield return null;
        }

        activeButtons.Dequeue();
        _currentButton.SetActive(false);
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
        else if (_buttonIndex >= 7 && _buttonIndex <= 10)
        {
            return rightArrow;
        }
        else
        {
            return downArrow;
        }
    }




}
