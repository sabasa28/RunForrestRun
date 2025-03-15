using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPausePanel : MonoBehaviour
{
    [SerializeField] GameObject ConfigurationPanel;
    [SerializeField] GameObject ControlsPanel;
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameplayController GC;
    [SerializeField] Slider MusicVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;

    private void Start()
    {
        MusicVolumeSlider.value = AudioManager.Get().musicVolume;
        SFXVolumeSlider.value = AudioManager.Get().sfxVolume;
    }

    private void OnEnable()
    {
        PausePanel.SetActive(true);
        ControlsPanel.SetActive(false);
        ConfigurationPanel.SetActive(false);
    }
    public void UpdateMusicVolume()
    {
        AudioManager.Get().UpdateMusicVolume(MusicVolumeSlider.value);
    }

    public void UpdateSFXVolume()
    {
        AudioManager.Get().sfxVolume = SFXVolumeSlider.value;
    }

    public void ChangePauseState(bool newState)
    {
        GC.ChangePauseState(newState);
    }

    public void GoBackToMenu()
    {
        GC.GoToMainMenu();
    }

    public void ShowConfig(bool shouldShow)
    {
        ConfigurationPanel.SetActive(shouldShow);
        PausePanel.SetActive(!shouldShow);
    }

    public void ShowControls(bool shouldShow)
    {
        ControlsPanel.SetActive(shouldShow);
        PausePanel.SetActive(!shouldShow);
    }
}
