using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    public Slider VolumeSlider;
    public Slider SFXSlider;
    public TMP_Dropdown Dropdown;
    public GameObject SettingsPanel;
    private List<string> fightOptions;
    public void Awake()
    {
        fightOptions = new(FightSettings.StringToSettings.Keys);
    }

    public void OnEnable()
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Main")) return;
        VolumeSlider.value = GameSettings.MusicVolume;
        SFXSlider.value = GameSettings.SFXVolume;
        Dropdown.options.Clear();
        Dropdown.AddOptions(fightOptions);
    }

    public void OnSettingsClick()
    {
        if (SettingsPanel.activeInHierarchy) return;
        VolumeSlider.value = GameSettings.MusicVolume;
        SFXSlider.value = GameSettings.SFXVolume;
        Dropdown.options.Clear();
        Dropdown.AddOptions(fightOptions);
        SettingsPanel.SetActive(true);
    }

    public void OnSettingsExit() => SettingsPanel.SetActive(false);

    public void OnIngameSettingsExit() => PopupQuestMenu.Instance.ToggleSettings();

    public void OnDifficultyChange()
    {
        var option = Dropdown.options[Dropdown.value];
        if (FightSettings.StringToSettings.TryGetValue(option.text, out var settings))
            GameSettings.Difficulty = settings;
    }

    public void OnExit() => Application.Quit();

    public void OnVolumeChange()
    {
        GameSettings.MusicVolume = VolumeSlider.value;
    }

    public void OnSFXVolumeChange() => GameSettings.SFXVolume = SFXSlider.value;

    public void OnMainMenuClick() => SceneManager.LoadScene("MainMenu");
}