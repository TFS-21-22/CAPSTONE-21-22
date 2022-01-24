using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager instance;
    [SerializeField] private Strafe strafeScript;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioSource music;

    public bool isPaused = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }


    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        ////////for hayden spline testing
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Time.timeScale = 2;
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Time.timeScale = 3;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Time.timeScale = 1;
        }
        //////////

        if (Input.GetKeyDown(KeyCode.P))
        {
            //Debug.Log("Escape key was pressed");
            if (!isPaused)
            {
                isPaused = true;
            }
            else
            {
                isPaused = false;

                //Enable Pause Menu UI
                pauseMenu.SetActive(false);

                //Disable Audio + BeatMaster
                music.UnPause();

                //Disable Canvas
                RhythmCanvas.instance.enabled = true;

                //Disable Movement
                strafeScript.enabled = true;

                //Disable beatmaster
                //BeatMaster.instance.enabled = true;

            }
        }
       
        if(isPaused)
        {
            //Enable Pause Menu UI
            pauseMenu.SetActive(true);

            //Disable Audio + BeatMaster
            music.Pause();
            
            //Disable Canvas
            RhythmCanvas.instance.enabled = false;

            //Disable Movement
            strafeScript.enabled = false;

            //BeatMaster.instance.enabled = false;

        }
        
    }
}
