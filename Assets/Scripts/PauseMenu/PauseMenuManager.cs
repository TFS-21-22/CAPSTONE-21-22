using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject newQuickTimeEvent;

    [SerializeField] private RhythmCanvas rhythmCanvas;
    [SerializeField] private Strafe strafeScript;
    [SerializeField] private AudioSource music;
    //[SerializeField] private AudioSource waterSFX;
    [SerializeField] private AudioSource fireAmabience;
    [SerializeField] private AudioSource forestAmbience;
    [SerializeField] private Slider musicVolumeSlider;
    //[SerializeField] private Slider waterVolumeSlider;

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
    [SerializeField] private GameObject HUD;

    //Resolutons
    [SerializeField] private TMPro.TMP_Dropdown resolutionsDropdown;
    [SerializeField] private Toggle resolutionOne;      //1920 x 1080
    [SerializeField] private Toggle resolutionTwo;      //1360 x 764
    [SerializeField] private Toggle resolutionThree;    //1280 x 720
    [SerializeField] private Toggle resolutionFour;     //1152 x 648

    private float defaultVolume = 0.5f;


    public bool isPaused = false;


    void Start()
    {
        QualitySettings.vSyncCount = 0;

        //Set Volume to 0.5f
       // waterSFX.volume = GameManager.instance.SFXVolume;
      //  waterVolumeSlider.value = GameManager.instance.SFXVolume;
        musicVolumeSlider.value = GameManager.instance.musicVolume;
        music.volume = GameManager.instance.musicVolume;
        fullscreenToggle.isOn = GameManager.instance.screenMode;

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
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    Application.Quit();
        //}

        //////////for hayden spline testing //HUD contains if-else statement if game is paused or not
        //if (Input.GetKeyDown(KeyCode.KeypadPlus))
        //{
        //    Time.timeScale = 2;
        //}
        //if (Input.GetKeyDown(KeyCode.KeypadEnter))
        //{
        //    Time.timeScale = 3;
        //}
        //if (Input.GetKeyDown(KeyCode.KeypadMinus))
        //{
        //    Time.timeScale = 1;
        //}
        //if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        //{
        //    //Time.timeScale = 0.2f;
        //}
        //////////

        if (Input.GetKeyDown(KeyCode.Escape) && music.isPlaying)
        {
            if(!newQuickTimeEvent.activeSelf && !rhythmCanvas.gameObject.activeSelf)
            {
                if (!isPaused)
                {
                    isPaused = true;

                    //Enable Pause Menu UI
                    pauseMenu.SetActive(true);
                    HUD.SetActive(false);

                    //Disable Audio + BeatMaster
                    music.Pause();
                   // waterSFX.Pause();
                    fireAmabience.Pause();
                    forestAmbience.Pause();

                    //Disable Canvas
                    if (rhythmCanvas.gameObject.activeSelf)
                        rhythmCanvas.gameObject.SetActive(false);

                    //Disable Movement
                    strafeScript.enabled = false;

                }
                else
                {
                    isPaused = false;

                    //Disable Movement
                    strafeScript.enabled = true;

                    //Enable Pause Menu UI
                    settingsMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    HUD.SetActive(true);

                    //Disable Audio + BeatMaster
                    music.UnPause();
                   // waterSFX.UnPause();
                    fireAmabience.UnPause();
                    forestAmbience.UnPause();


                    //Disable Canvas
                    if (rhythmCanvas.gameObject.activeSelf)
                        rhythmCanvas.gameObject.SetActive(true);


                    //Disable beatmaster
                    BeatMaster.instance.enabled = true;

                }
            }
            
        }   
    }

    public void EnableGame()
    {
        isPaused = false;

        //Enable Pause Menu UI
        pauseMenu.SetActive(false);
        HUD.SetActive(true);

        //Disable Audio + BeatMaster
        music.UnPause();

        //Disable Canvas
        if (!rhythmCanvas.gameObject.activeSelf)
            rhythmCanvas.enabled = true;

        //Disable Movement
        strafeScript.enabled = true;

        //Disable beatmaster
        BeatMaster.instance.enabled = true;
    }

    private void ApplyGraphics()
    {
        music.volume = musicVolumeSlider.value;
        //waterSFX.volume = waterVolumeSlider.value;
        GameManager.instance.musicVolume = musicVolumeSlider.value;
        GameManager.instance.screenMode = fullscreenToggle.isOn;

        if (vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        if(resolutionsDropdown.value == 0)
        {
            Screen.SetResolution(1980, 1080, fullscreenToggle.isOn);
        } else if(resolutionsDropdown.value == 1)
        {
            Screen.SetResolution(1360, 764, fullscreenToggle.isOn);
        }
        else if(resolutionsDropdown.value == 2)
        {
            Screen.SetResolution(1280, 720, fullscreenToggle.isOn);
        }
        else if(resolutionsDropdown.value == 3)
        {
            Screen.SetResolution(1152, 648, fullscreenToggle.isOn);
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
        HUD.GetComponent<HUD>().collObj.SetActive(false);
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

        //Disable Movement
        strafeScript.enabled = true;

        //Enable Pause Menu UI
        pauseMenu.SetActive(false);
        HUD.SetActive(true);

        //Disable Audio + BeatMaster
        music.UnPause();
       // waterSFX.UnPause();

        //Disable Canvas
        if (!rhythmCanvas.gameObject.activeSelf)
            rhythmCanvas.enabled = true;

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
