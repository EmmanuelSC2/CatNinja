using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class CanvasManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Button")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public Button backButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button returnToMenuButton;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    [Header("Text")]
    public TMP_Text livesText;
    public TMP_Text masterVolSliderText;
    public TMP_Text musicVolSliderText;
    public TMP_Text sfxVolSliderText;

    [Header("Slider")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    // Start is called before the first frame update

    void Start()
    {
        if (quitButton)
            quitButton.onClick.AddListener(Quit);

        if (playButton)
            playButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(1));

        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(0));

        if (settingsButton)
            settingsButton.onClick.AddListener(() => SetMenus(settingsMenu, mainMenu));

        if (backButton)
            backButton.onClick.AddListener(() => SetMenus(mainMenu, settingsMenu));

        if (resumeButton)
            resumeButton.onClick.AddListener(() => {
                SetMenus(null, pauseMenu);
                Time.timeScale = 1;
            });

        if (masterVolSlider)
        {
            masterVolSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, masterVolSliderText, "MasterVol"));
            float newValue;
            audioMixer.GetFloat("MasterVol", out newValue);
            masterVolSlider.value = newValue + 80;
            if (masterVolSliderText)
                masterVolSliderText.text = masterVolSlider.value.ToString();
        }

        if (musicVolSlider)
        {
            musicVolSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, musicVolSliderText, "MusicVol"));
            float newValue;
            audioMixer.GetFloat("MusicVol", out newValue);
            musicVolSlider.value = newValue + 80;
            if (musicVolSliderText)
                musicVolSliderText.text = musicVolSlider.value.ToString();
        }

        if (sfxVolSlider)
        {
            sfxVolSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, sfxVolSliderText, "SFXVol"));
            float newValue;
            audioMixer.GetFloat("SFXVol", out newValue);
            sfxVolSlider.value = newValue + 80;
            if (sfxVolSliderText)
                sfxVolSliderText.text = sfxVolSlider.value.ToString();
        }

        if (livesText)
        {
            GameManager.Instance.OnLivesValueChanged.AddListener(UpdateLifeText);
            livesText.text = "Lives: " + GameManager.Instance.lives.ToString();
        }
    }

    void UpdateLifeText(int value)
    {
        livesText.text = "Lives: " + value.ToString();
    }

    void SetMenus(GameObject menuToActivate, GameObject menuToInactivate)
    {
        if (menuToActivate)
            menuToActivate.SetActive(true);

        if (menuToInactivate)
            menuToInactivate.SetActive(false);
    }

    void OnSliderValueChanged(float value, TMP_Text volSliderText, string sliderName)
    {
        volSliderText.text = value.ToString();
        audioMixer.SetFloat(sliderName, value - 80);
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            SetPause();
        }
    }

    void SetPause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}

