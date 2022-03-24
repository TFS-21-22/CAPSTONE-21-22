using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.Events;

public class TutorialText : MonoBehaviour
{
    public GameObject[] tutorialText = new GameObject[5];

    int index = 0;

    bool textActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!textActive && index <= tutorialText.Length)
        {
            StartCoroutine(MoveText());
        }

        if(!textActive && index == tutorialText.Length)
        {
            Destroy(this);
        }
    }

    IEnumerator MoveText()
    {
        if(CPManager.instance.checkPoint == 0)
        {
            textActive = true;

            tutorialText[index].SetActive(true);

            yield return new WaitForSeconds(5.0f);

            tutorialText[index].SetActive(false);

            index++;

            textActive = false;

        }

    }
}
