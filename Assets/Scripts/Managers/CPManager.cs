using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


public class CPManager : MonoBehaviour
{
    static CPManager _instance = null;
    public static CPManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public int checkPoint;
    public float scoreSaved;
    public bool[] collectables;
    public bool[] collectablesSaved;

    public HUD hud;

    //public PathCreator pathCreator;
    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        //Player.pathCreator = Player.otherPaths[0];
        checkPoint = 0;
    }

    //Beatmaster script contains the rest of the CP code
    // Update is called once per frame
    void Update()
    {
        hud = FindObjectOfType<HUD>();


        if (Input.GetKeyDown("1"))
        {
            checkPoint = 0;
            BeatMaster.instance.beatCount = 0;
            GameManager.instance.health = 100;
            SceneManager.LoadScene("LevelDesignBlockout");
        }
        if (Input.GetKeyDown("2"))
        {
            checkPoint = 1;
            BeatMaster.instance.beatCount = 100;
            GameManager.instance.health = 100;
            SceneManager.LoadScene("LevelDesignBlockout");
        }
        if (Input.GetKeyDown("3"))
        {
            checkPoint = 2;
            BeatMaster.instance.beatCount = 278;
            GameManager.instance.health = 100;
            SceneManager.LoadScene("LevelDesignBlockout");
        }

        //INSTA-LOSE BUTTON
        if (Input.GetKeyDown("0"))
        {
            SceneManager.LoadScene("LevelDesignBlockout");
        }
    }
}
