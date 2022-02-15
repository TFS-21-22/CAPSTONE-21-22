using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewQTEEvent : MonoBehaviour
{
    [SerializeField] private Image circle;
    [SerializeField] private GameObject BG;
    [SerializeField] private GameObject[] direction; //1 - Left // 2 - Right //3 - Up // 4 - Down
    [SerializeField] private GameObject okayResult;
    [SerializeField] private GameObject goodResult;
    [SerializeField] private GameObject perfectResult;

    private float maxFillAmount = 1f;
    private int sequenceCount = 0;

    private bool leftKey = false;
    private bool rightKey = false;
    private bool upKey = false;
    private bool downKey = false;

    [SerializeField] private Strafe strafe;
    // Start is called before the first frame update
    private void Start()
    {
        //direction = new GameObject[4];
        BeatMaster.Beat += BeatCheckQTE;
    }

    private void Update()
    {
        
    }

    private void BeatCheckQTE(int beat)
    {
        if ((beat + 3) % 4 == 0)
        {
            StartCoroutine(QTE_Enable());
        }
    }

    IEnumerator QTE_Enable()
    {
        strafe.enabled = false;
        circle.gameObject.SetActive(true);
        BG.SetActive(true);
        if (!leftKey && !rightKey && !upKey && !downKey)
        {
            RandomKey();
        }

        //Debug.Log("LEFT:" + leftKey + " RIGHT:" + rightKey + " UP:" + upKey + " DOWN:" + downKey);
        while (circle.fillAmount > 0)
        {
            circle.fillAmount -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.LeftArrow) && leftKey)
            {
                DisplayResult(sequenceCount);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && rightKey)
            {
                DisplayResult(sequenceCount);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && upKey)
            {
                DisplayResult(sequenceCount);
         
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && downKey)
            {
                DisplayResult(sequenceCount);
              
            }
            yield return null;
        }
        
        ResetQTE();
    }

    private void RandomKey()
    {
        var rand = Random.Range(0, 4);

        if(rand == 0)
        {
            direction[rand].gameObject.SetActive(true);
            leftKey = true;
        }
        else if(rand == 1)
        {
            direction[rand].gameObject.SetActive(true);
            rightKey = true;
        }
        else if(rand == 2)
        {
            direction[rand].gameObject.SetActive(true);
            upKey = true;
        }
        else
        {
            direction[rand].gameObject.SetActive(true);
            downKey = true;
        }
    }

    private void ResetQTE()
    {
        circle.fillAmount = maxFillAmount;
        circle.gameObject.SetActive(false);
        BG.SetActive(false);

        foreach (GameObject _direction in direction)
        {
            _direction.gameObject.SetActive(false);
        }

        rightKey = false;
        leftKey = false;
        upKey = false;
        downKey = false;
        
        if(sequenceCount >= 2)
        {
            sequenceCount = 0;
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

        if (perfectResult.activeSelf)
        {
            perfectResult.SetActive(false);
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
            perfectResult.SetActive(true);
        }

    }
}
