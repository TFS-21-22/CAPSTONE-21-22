using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject MainBG;

    //Buttons
    [SerializeField] private Button startButton;
    [SerializeField] private Button compendiumButton;
    [SerializeField] private Button compendiumnBackButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button videoSettingsButton;
    [SerializeField] private Button AudioButton;
    [SerializeField] private Button controlsButton;

    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Button applyButton;

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider waterVolumeSlider;

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject compendiumMenu;

    [SerializeField] private GameObject videoSettingsMenuParent;
    [SerializeField] private GameObject AudioParent;
    [SerializeField] private GameObject controlsSettingsMenuParent;

    //Resolutons
    [SerializeField] private TMPro.TMP_Dropdown resolutionsDropdown;
    [SerializeField] private Toggle resolutionOne;      //1920 x 1080
    [SerializeField] private Toggle resolutionTwo;      //1360 x 764
    [SerializeField] private Toggle resolutionThree;    //1280 x 720
    [SerializeField] private Toggle resolutionFour;     //1152 x 648

    void Awake()
    {
        Time.timeScale = 1;

        if (startButton)
        {
            startButton.onClick.AddListener(delegate { GameManager.instance.StartGame(); });
        }

        if (exitButton)
        {
            exitButton.onClick.AddListener(delegate { GameManager.instance.QuitGame(); });
        }
        if (settingsButton)
        {
            settingsButton.onClick.AddListener(EnableSettingsMenu);
        }

        if (compendiumButton)
        {
            compendiumButton.onClick.AddListener(EnableCompendium);
        }

        if (settingsBackButton)
        {
            settingsBackButton.onClick.AddListener(DisableSettings);
        }

        if (compendiumnBackButton)
        {
            compendiumnBackButton.onClick.AddListener(DisableCompendium);
        }

        if (applyButton)
        {
            applyButton.onClick.AddListener(ApplySettings);
        }

        if (controlsButton)
        {
            controlsButton.onClick.AddListener(controlsMenu);
        }
        if (AudioButton)
        {
            AudioButton.onClick.AddListener(Audiosetting);
        }
        if (videoSettingsButton)
        {
            videoSettingsButton.onClick.AddListener(videoSettingsMenu);
        }
        if (GameManager.instance)
        {
            waterVolumeSlider.value = GameManager.instance.SFXVolume;
            musicVolumeSlider.value = GameManager.instance.musicVolume;
            fullscreenToggle.isOn = GameManager.instance.screenMode;

            if (GameManager.instance.resolution == 4)
            {
                MainBG.transform.localScale = new Vector3(2, 2, 1);
                settingsMenu.transform.localScale = new Vector3(2, 2, 1);
                compendiumMenu.transform.localScale = new Vector3(2, 2, 1);
            }
            else
            {
                MainBG.transform.localScale = new Vector3(1, 1, 1);
                settingsMenu.transform.localScale = new Vector3(1, 1, 1);
                compendiumMenu.transform.localScale = new Vector3(1, 1, 1);
            }
        }

    }
    void Start()
    {
        
    }

    private void ApplySettings()
    {
        GameManager.instance.SFXVolume = waterVolumeSlider.value;
        GameManager.instance.musicVolume = musicVolumeSlider.value;
        GameManager.instance.screenMode = fullscreenToggle.isOn;
        GameManager.instance.resolution = resolutionsDropdown.value;
        if (vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }


        if (resolutionsDropdown.value == 0)
        {
            MainBG.transform.localScale = new Vector3(1, 1, 1);
            settingsMenu.transform.localScale = new Vector3(1, 1, 1);
            compendiumMenu.transform.localScale = new Vector3(1, 1, 1);
            Screen.SetResolution(1980, 1080, fullscreenToggle.isOn);
        }
        else if (resolutionsDropdown.value == 1)
        {
            MainBG.transform.localScale = new Vector3(1, 1, 1);
            settingsMenu.transform.localScale = new Vector3(1, 1, 1);
            compendiumMenu.transform.localScale = new Vector3(1, 1, 1);
            Screen.SetResolution(1360, 764, fullscreenToggle.isOn);
        }
        else if (resolutionsDropdown.value == 2)
        {
            MainBG.transform.localScale = new Vector3(1, 1, 1);
            settingsMenu.transform.localScale = new Vector3(1, 1, 1);
            compendiumMenu.transform.localScale = new Vector3(1, 1, 1);
            Screen.SetResolution(1280, 720, fullscreenToggle.isOn);
        }
        else if (resolutionsDropdown.value == 3)
        {
            MainBG.transform.localScale = new Vector3(1, 1, 1);
            settingsMenu.transform.localScale = new Vector3(1, 1, 1);
            compendiumMenu.transform.localScale = new Vector3(1, 1, 1);
            Screen.SetResolution(1152, 648, fullscreenToggle.isOn);
        }
        else if (resolutionsDropdown.value == 4)
        {
            MainBG.transform.localScale = new Vector3(2, 2, 1);
            settingsMenu.transform.localScale = new Vector3(2, 2, 1);
            compendiumMenu.transform.localScale = new Vector3(2, 2, 1);
            Screen.SetResolution(3840, 2160, fullscreenToggle.isOn);
        }

    }

    private void DisableCompendium()
    {
        compendiumMenu.SetActive(false);
    }

    private void DisableSettings()
    {
        Title.SetActive(true);
        MainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        videoSettingsMenuParent.SetActive(false);
        AudioParent.SetActive(false);
        controlsSettingsMenuParent.SetActive(false);
    }

    private void EnableCompendium()
    {
        compendiumMenu.SetActive(true);
    }


    private void EnableSettingsMenu()
    {
        Title.SetActive(false);
        MainMenu.SetActive(false);
        waterVolumeSlider.value = 0.5f;
        musicVolumeSlider.value = 0.5f;
        fullscreenToggle.isOn = true;
        settingsMenu.SetActive(true);
        videoSettingsMenuParent.SetActive(true);
        AudioParent.SetActive(false);
        controlsSettingsMenuParent.SetActive(false);
    }

    private void videoSettingsMenu()
    {
        videoSettingsMenuParent.SetActive(true);
        AudioParent.SetActive(false);
        controlsSettingsMenuParent.SetActive(false);
    }

    private void controlsMenu()
    {
        controlsSettingsMenuParent.SetActive(true);
        videoSettingsMenuParent.SetActive(false);
        AudioParent.SetActive(false);
    }
    private void Audiosetting()
    {
        AudioParent.SetActive(true);
        videoSettingsMenuParent.SetActive(false);
        controlsSettingsMenuParent.SetActive(false);

    }
}
