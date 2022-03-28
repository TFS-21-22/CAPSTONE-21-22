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
    public bool koreoReader;

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

    public void Update()
    {
        if (koreoReader == false)
        {
            StartCoroutine(KoreoSwitch());
        }
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
        Time.timeScale = 1;
        health = 100f;
        SceneManager.LoadScene("LevelDesignBlockout");
    }

    IEnumerator KoreoSwitch()
    {
        koreoReader = false;
        yield return new WaitForSeconds(1);
        koreoReader = true;
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
        CPManager.instance.scoreSaved = 0;
        SceneManager.LoadScene("Title Screen");
    }
}
