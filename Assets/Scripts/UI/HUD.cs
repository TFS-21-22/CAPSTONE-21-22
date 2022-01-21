using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public float score;
    public float collectables;
    public float time;
    public ScoreSystem scoreSystem;

    public TMP_Text scoreText;
    public TMP_Text timeText;
    public TMP_Text collText;

    public GameObject pauseMenu;
    public GameObject hud;

    // Start is called before the first frame update
    void Start()
    {
        time += Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //SCORE
        score = scoreSystem.score;

        scoreText.text = score.ToString();

        //TIMER
        

       
        float t = Time.time - time;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f0");

        timeText.text = minutes + ":" + seconds;

        //COLLECTABLES
        collText.text = collectables.ToString();
        
        
    }
}
