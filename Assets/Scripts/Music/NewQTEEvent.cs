using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewQTEEvent : MonoBehaviour
{
    [SerializeField] private Image circle;
    [SerializeField] private GameObject[] BG;           //0 = red /1 = blue //2 = green //3 = purple  
    [SerializeField] private GameObject[] direction;    //0 - Left // 1 - Right //2 - Up // 3 - Down
    [SerializeField] private GameObject okayResult;
    [SerializeField] private GameObject goodResult;
    [SerializeField] private GameObject niceResult;
    [SerializeField] private GameObject perfectResult;

    private int[] redPattern = new int[4] { 0, 0, 2, 1 };   //0
    private int[] bluePattern = new int[4] { 2, 2, 3, 3 };  //1
    private int[] greenPattern = new int[4] { 1, 1, 0, 2 }; //2
    private int[] purplePattern = new int[4] { 0, 1, 2, 3 };//3


    private int index = 0;
    private float maxFillAmount = 1f;
    private int sequenceCount = 0;
    private int sequencesHit = 0;
    private int chosenPattern;

    private bool leftKey = false;
    private bool rightKey = false;
    private bool upKey = false;
    private bool downKey = false;

    private bool pressed = false;

    [SerializeField] private Strafe strafe;
    // Start is called before the first frame update

    private void BeatCheckQTE(int beat)
    {
        if ((beat + 3) % 4 == 0)
        {
            if(this.gameObject.activeSelf)
            StartCoroutine(QTE_Enable());
        }
    }

    IEnumerator QTE_Enable()
    {
        if (sequenceCount == 0)
        {
            var randomPattern = Random.Range(0, 4);
            chosenPattern = randomPattern;
        }
        
        circle.gameObject.SetActive(true);
        
        BG[chosenPattern].SetActive(true);

        

        if (!leftKey && !rightKey && !upKey && !downKey)
        {
            if(chosenPattern == 0)
            {
                RandomKey(redPattern[index]);
            }
            else if(chosenPattern == 1)
            {
                RandomKey(bluePattern[index]);
            }
            else if (chosenPattern == 2)
            {
                RandomKey(greenPattern[index]);
            }
            else if (chosenPattern == 3)
            {
                RandomKey(purplePattern[index]);
            }
        }

        while (circle.fillAmount > 0)
        {
            circle.fillAmount -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.LeftArrow) && leftKey && !pressed)
            {
                pressed = true;
                DisplayResult(sequencesHit);
                sequencesHit++;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && rightKey && !pressed)
            {
                pressed = true;
                DisplayResult(sequencesHit);
                sequencesHit++;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && upKey && !pressed)
            {
                pressed = true;
                DisplayResult(sequencesHit);
                sequencesHit++;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && downKey && !pressed)
            {
                pressed = true;
                DisplayResult(sequencesHit);
                sequencesHit++;
            }
            yield return null;
        }
        
        ResetQTE();
    }

    private void RandomKey(int _direction)
    {
        if (_direction == 0)
        {
            direction[_direction].gameObject.SetActive(true);
            leftKey = true;
            index++;
        }
        else if(_direction == 1)
        {
            direction[_direction].gameObject.SetActive(true);
            rightKey = true;
            index++;
        }
        else if(_direction == 2)
        {
            direction[_direction].gameObject.SetActive(true);
            upKey = true;
            index++;
        }
        else
        {
            direction[_direction].gameObject.SetActive(true);
            downKey = true;
            index++;
        }
    }

    private void ResetQTE()
    {
        pressed = false;
        circle.fillAmount = maxFillAmount;
        circle.gameObject.SetActive(false);
        BG[chosenPattern].SetActive(false);

        foreach (GameObject _direction in direction)
        {
            _direction.gameObject.SetActive(false);
        }

        rightKey = false;
        leftKey = false;
        upKey = false;
        downKey = false;
        
        if(sequenceCount >= 3)
        {
            sequencesHit = 0;
            sequenceCount = 0;
            index = 0;
            strafe.enabled = true;
            this.gameObject.SetActive(false);
        }
        else
        {
            sequenceCount++;
        }

        if(okayResult.activeSelf)
        {
            okayResult.SetActive(false);
        }

        if (goodResult.activeSelf)
        {
            goodResult.SetActive(false);
        }

        if (niceResult.activeSelf)
        {
            niceResult.SetActive(false);
        }

    }

    private void DisplayResult(int _sequenceCount)
    {
        if(_sequenceCount == 0)
        {
            //Okay
            okayResult.SetActive(true);
        }
        else if(_sequenceCount == 1)
        {
            //Good
            goodResult.SetActive(true);
        }
        else if(_sequenceCount == 2)
        {
            //Perfect
            niceResult.SetActive(true);
        }
        else if(_sequenceCount == 3)
        {
            perfectResult.SetActive(true);
        }

    }
}
