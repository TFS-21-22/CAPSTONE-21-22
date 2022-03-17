using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    static GameManager _instance = null;
    public static GameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public float health = 100f;
    public float maxHealth = 100f;
    public bool screenMode = true;
    public float musicVolume;
    public float SFXVolume;
    // Start is called before the first frame update
    void Start()
    {
        if (musicVolume < 0)
        {
            musicVolume = 0.5f;
        }
        if (SFXVolume < 0)
        {
            SFXVolume = 0.5f;
        }

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
        
        health = 3;
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
