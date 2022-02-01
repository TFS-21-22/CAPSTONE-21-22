using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager instance;
    [SerializeField] private Strafe strafeScript;
    [SerializeField] private AudioSource music;

    //Buttons
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button compendiumButton;
    [SerializeField] private Button compendiumnBackButton;
    [SerializeField] private Button titlescreenButton;

    //Menu
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject compendiumMenu;
    

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

    void Start()
    {
        if(resumeButton)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }

        if(settingsButton)
        {
            settingsButton.onClick.AddListener(EnableSettingsMenu);
        }

        if(compendiumButton)
        {
            compendiumButton.onClick.AddListener(EnableCompendium);
        }

        if(titlescreenButton)
        {
            titlescreenButton.onClick.AddListener(LoadTitlescreen);
        }

        if(settingsBackButton)
        {
            settingsBackButton.onClick.AddListener(DisableSettings);
        }

        if(compendiumnBackButton)
        {
            compendiumnBackButton.onClick.AddListener(DisableCompendium);
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
                if(!RhythmCanvas.instance.gameObject.activeSelf)
                    RhythmCanvas.instance.enabled = true;

                //Disable Movement
                strafeScript.enabled = true;

                //Disable beatmaster
                BeatMaster.instance.enabled = true;

            }
        }
       
        if(isPaused)
        {
            //Enable Pause Menu UI
            pauseMenu.SetActive(true);

            //Disable Audio + BeatMaster
            music.Pause();

            //Disable Canvas
            if (RhythmCanvas.instance.gameObject.activeSelf)
                RhythmCanvas.instance.enabled = false;

            //Disable Movement
            strafeScript.enabled = false;

            //BeatMaster.instance.enabled = false;
        }

       
        
    }

    public void EnableGame()
    {
        isPaused = false;

        //Enable Pause Menu UI
        pauseMenu.SetActive(false);

        //Disable Audio + BeatMaster
        music.UnPause();

        //Disable Canvas
        if (!RhythmCanvas.instance.gameObject.activeSelf)
            RhythmCanvas.instance.enabled = true;

        //Disable Movement
        strafeScript.enabled = true;

        //Disable beatmaster
        BeatMaster.instance.enabled = true;
    }

    void DisableCompendium()
    {
        compendiumMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    void DisableSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    void EnableCompendium()
    {
        compendiumMenu.SetActive(true);
    }

    void EnableSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }

    void LoadTitlescreen()
    {
        
    }

    void ResumeGame()
    {
        isPaused = false;

        //Enable Pause Menu UI
        pauseMenu.SetActive(false);

        //Disable Audio + BeatMaster
        music.UnPause();

        //Disable Canvas
        if (!RhythmCanvas.instance.gameObject.activeSelf)
            RhythmCanvas.instance.enabled = true;

        //Disable Movement
        strafeScript.enabled = true;

        //Disable beatmaster
        BeatMaster.instance.enabled = true;
    }
}
