using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private RhythmCanvas rhythmCanvas;
    public static PauseMenuManager instance;
    [SerializeField] private Strafe strafeScript;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource waterSFX;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider waterVolumeSlider;


    //Buttons
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button compendiumButton;
    [SerializeField] private Button compendiumnBackButton;

    //Settings Buttons
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button videoSettingsButton;
    [SerializeField] private Button volumeSettingsButton;

    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Button applyGraphics;

    //Settings Menus
    [SerializeField] private GameObject videoSettingsMenuParent;
    [SerializeField] private GameObject volumeSettingsMenuParent;
    [SerializeField] private GameObject controlsSettingsMenuParent;


    //Menu
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject compendiumMenu;

    //Resolutons
    [SerializeField] private Toggle resolutionOne;      //1920 x 1080
    [SerializeField] private Toggle resolutionTwo;      //1360 x 764
    [SerializeField] private Toggle resolutionThree;    //1280 x 720
    [SerializeField] private Toggle resolutionFour;     //1152 x 648

    private float defaultVolume = 0.5f;


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
        QualitySettings.vSyncCount = 0;

        //Set Volume to 0.5f
        waterSFX.volume = defaultVolume;
        waterVolumeSlider.value = defaultVolume;
        musicVolumeSlider.value = defaultVolume;
        music.volume = defaultVolume;

        if (resumeButton)
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

        if(settingsBackButton)
        {
            settingsBackButton.onClick.AddListener(DisableSettings);
        }

        if(compendiumnBackButton)
        {
            compendiumnBackButton.onClick.AddListener(DisableCompendium);
        }

        if(applyGraphics)
        {
            applyGraphics.onClick.AddListener(ApplyGraphics);
        }

        if (controlsButton)
        {
            controlsButton.onClick.AddListener(controlsMenu);
        }

        if (videoSettingsButton)
        {
            videoSettingsButton.onClick.AddListener(videoSettingsMenu);
        }

        if (controlsButton)
        {
            controlsButton.onClick.AddListener(controlsMenu);
        }

        if (volumeSettingsButton)
        {
            volumeSettingsButton.onClick.AddListener(volumeSettings);
        }
    }


    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
        if(resolutionOne.isOn)
        {
            resolutionTwo.isOn = false;
            resolutionThree.isOn = false;
            resolutionFour.isOn = false;

        }
        else if (resolutionTwo.isOn)
        {
            resolutionOne.isOn = false;
            resolutionThree.isOn = false;
            resolutionFour.isOn = false;

        }
        else if (resolutionThree.isOn)
        {
            resolutionOne.isOn = false;
            resolutionTwo.isOn = false;
            resolutionFour.isOn = false;

        }
        else if (resolutionFour.isOn)
        {
            resolutionOne.isOn = false;
            resolutionTwo.isOn = false;
            resolutionThree.isOn = false;

        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        ////////for hayden spline testing //HUD contains if-else statement if game is paused or not
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
            if (!isPaused)
            {
                isPaused = true;

                //Enable Pause Menu UI
                pauseMenu.SetActive(true);

                //Disable Audio + BeatMaster
                music.Pause();

                waterSFX.Pause();

                //Disable Canvas
                if (rhythmCanvas.gameObject.activeSelf)
                    rhythmCanvas.gameObject.SetActive(false);

                //Disable Movement
                strafeScript.enabled = false;
            }
            else
            {
                isPaused = false;

                //Enable Pause Menu UI
                pauseMenu.SetActive(false);

                //Disable Audio + BeatMaster
                music.UnPause();
                waterSFX.UnPause();


                //Disable Canvas
                if (rhythmCanvas.gameObject.activeSelf)
                    rhythmCanvas.gameObject.SetActive(true);

                //Disable Movement
                strafeScript.enabled = true;

                //Disable beatmaster
                BeatMaster.instance.enabled = true;

            }
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

    private void ApplyGraphics()
    {
        music.volume = musicVolumeSlider.value;
        waterSFX.volume = waterVolumeSlider.value;

        if (vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        fullscreenToggle.isOn = Screen.fullScreen;

        if (resolutionOne.isOn)
        {
            Screen.SetResolution(1980, 1080, fullscreenToggle.isOn = Screen.fullScreen);

        }
        else if (resolutionTwo.isOn)
        {
            Screen.SetResolution(1360, 764, fullscreenToggle.isOn = Screen.fullScreen);

        }
        else if (resolutionThree.isOn)
        {
            Screen.SetResolution(1280, 720, fullscreenToggle.isOn = Screen.fullScreen);

        }
        else if (resolutionFour.isOn)
        {
            Screen.SetResolution(1152, 648, fullscreenToggle.isOn = Screen.fullScreen);
        }
    }

    private void DisableCompendium()
    {
        compendiumMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    private void DisableSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    private void EnableCompendium()
    {
        compendiumMenu.SetActive(true);
    }

    private void EnableSettingsMenu()
    {
        settingsMenu.SetActive(true);
        videoSettingsMenuParent.SetActive(false);
        volumeSettingsMenuParent.SetActive(false);
        controlsSettingsMenuParent.SetActive(false);
    }

    private void ResumeGame()
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

    private void videoSettingsMenu()
    {
        videoSettingsMenuParent.SetActive(true);
        volumeSettingsMenuParent.SetActive(false);
        controlsSettingsMenuParent.SetActive(false);

    }

    private void volumeSettings()
    {
        volumeSettingsMenuParent.SetActive(true);
        videoSettingsMenuParent.SetActive(false);
        controlsSettingsMenuParent.SetActive(false);
    }
    private void controlsMenu()
    {
        controlsSettingsMenuParent.SetActive(true);
        videoSettingsMenuParent.SetActive(false);
        volumeSettingsMenuParent.SetActive(false);

    }
}
