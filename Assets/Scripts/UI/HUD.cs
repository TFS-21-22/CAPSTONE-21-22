using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public float score;
    public float collectables;
    
    private float time;

    private int minCount;
    //private int secCount;

    public ScoreSystem scoreSystem;

    public TMP_Text scoreText;
    public TMP_Text timeText;
    public TMP_Text livesText;
    public TMP_Text collText;

    public GameObject pauseMenu;
    public GameObject hud;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //HEALTH
        livesText.text = "Lives: " + GameManager.instance.health;

        //SCORE
        score = scoreSystem.score;
        scoreText.text = score.ToString();

        //TIMER     
        time += Time.deltaTime;
        timeText.text = minCount.ToString("00") + ":" + ((int)time).ToString("00");

        if (time >= 60)
        {
            minCount++;
            time = 0;
        }

        //COLLECTABLES
        collText.text = collectables.ToString();
        
        
    }
}
