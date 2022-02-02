using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
    public float score;
    public float collectables;

    public ScoreSystem scoreSystem;

    public TMP_Text scoreText;
    public TMP_Text collectableTxt;

    public GameObject resultsScreen;

    public Button quitButton;
    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        resultsScreen.SetActive(false);

        if (quitButton)
        {
            quitButton.onClick.AddListener(() => GameManager.instance.QuitGame());
        }

        if (restartButton)
        {
            restartButton.onClick.AddListener(() => GameManager.instance.QuitGame());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //SCORE
        if(scoreSystem)
        {
            score = scoreSystem.score;
            scoreText.text = score.ToString();
        }
    

        //COLLECTABLES
        collectableTxt.text = collectables.ToString();
    }

    public void Quit()
    {

    }
}
