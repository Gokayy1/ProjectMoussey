using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void OnEnable()
    {
        if (AudioManager.Instance == null || AudioManager.Instance.musicSource == null || AudioManager.Instance.sfxSource == null)
        {
            Debug.LogWarning("AudioManager or its sources not ready.");
            return;
        }

    
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();

      
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

     
        musicSlider.SetValueWithoutNotify(AudioManager.Instance.musicSource.volume);
        sfxSlider.SetValueWithoutNotify(AudioManager.Instance.sfxSource.volume);

      //  Debug.Log("Sliders linked at runtime.");
    }
}
