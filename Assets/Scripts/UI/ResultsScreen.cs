using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsScreen : MonoBehaviour
{
    [Header("Score & Collectables")]
    public float score;
    public float collectables;

    public ScoreSystem scoreSystem;

    [Header("TMP Text")]
    public TMP_Text scoreText;
    public TMP_Text collectableTxt;

    [Header("Canvas")]
    public GameObject resultsScreen;
    public GameObject HUD;

    [Header("Buttons")]
    public Button quitButton;
    public Button restartButton;
    public Button returnToMenuButton;

    public bool endHit = false;

    // Start is called before the first frame update
    void Start()
    {
        resultsScreen.SetActive(false);

        endHit = false;

        if (quitButton)
        {
            quitButton.onClick.AddListener(() => GameManager.instance.QuitGame());
        }

        if (restartButton)
        {
            restartButton.onClick.AddListener(() => SceneManager.LoadScene("LevelDesignBlockout"));

        }

        if(returnToMenuButton)
        {
            returnToMenuButton.onClick.AddListener(() => SceneManager.LoadScene("Title Screen"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(resultsScreen)
        {
            if(endHit)
            {
                HUD.SetActive(false);

                resultsScreen.SetActive(true);

                Time.timeScale = 0;
            }
        }
        //SCORE
        if(scoreSystem)
        {
            score = scoreSystem.score;
            scoreText.text = score.ToString();
        }
    

        //COLLECTABLES
        collectableTxt.text = collectables.ToString();
    }

}
