using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button settingsButton;
    public Button backButton;
    public Button returnToMenuButton;
    public Button returnToGameButton;

    public Button compendiumButton;
    public Button compendiumnBackButton;

    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;
    public GameObject compendiumMenu;

    public Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (startButton)
        {
            startButton.onClick.AddListener(() => GameManager.instance.StartGame());
        }

        if (quitButton)
        {
            quitButton.onClick.AddListener(() => GameManager.instance.QuitGame());
        }

        if (returnToMenuButton)
        {
            returnToMenuButton.onClick.AddListener(() => GameManager.instance.ReturnToMenu());
        }

        if (backButton)
        {
            backButton.onClick.AddListener(() => ShowMainMenu());
        }

        if (settingsButton)
        {
            settingsButton.onClick.AddListener(() => ShowSettingsMenu());
        }

        if (returnToGameButton)
        {
            returnToGameButton.onClick.AddListener(() => ReturnToGame());
        }

        if (compendiumButton)
        {
            compendiumButton.onClick.AddListener(EnableCompendium);
        }
        if (compendiumnBackButton)
        {
            compendiumnBackButton.onClick.AddListener(DisableCompendium);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMenu)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                pauseMenu.SetActive(!pauseMenu.activeSelf);
            }

            Time.timeScale = pauseMenu.activeSelf ? Time.timeScale = 0 : Time.timeScale = 1;
        }
    }

    void ShowMainMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    void ShowSettingsMenu()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    private void DisableCompendium()
    {
        compendiumMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void EnableCompendium()
    {
        compendiumMenu.SetActive(true);
    }

    public void ReturnToGame()
    {
        pauseMenu.SetActive(false);
    }
}
