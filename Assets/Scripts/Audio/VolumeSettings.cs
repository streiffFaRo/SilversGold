using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";
    public const string MIXER_MASTER = "MasterVolume";
    
    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(VolumeManager.MASTER_KEY, 1f);
        musicSlider.value = PlayerPrefs.GetFloat(VolumeManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(VolumeManager.SFX_KEY, 1f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(VolumeManager.MASTER_KEY, masterSlider.value);
        PlayerPrefs.SetFloat(VolumeManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(VolumeManager.SFX_KEY, sfxSlider.value);
    }

    private void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    
    private void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
    
    private void SetMasterVolume(float value)
    {
        mixer.SetFloat(MIXER_MASTER, Mathf.Log10(value) * 20);
    }
}
