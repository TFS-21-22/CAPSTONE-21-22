using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    static GameManager _instance;
    public static GameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public float health = 100f;
    public float maxHealth = 100f;
    public bool screenMode = true;
    public float musicVolume = 0.5f;
    public float SFXVolume = 0.5f;
    public float resolution = 0;
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

        health = 100f;
    }


    public void StartGame()
    {
        if (CPManager.instance)
        {
            CPManager.instance.checkPoint = 0;
        }
        if (BeatMaster.instance)
        {
            BeatMaster.instance.beatCount = 0;
        }
        
        health = 100f;
        SceneManager.LoadScene("LevelDesignBlockout");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }
}
