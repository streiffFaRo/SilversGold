using System;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour //Läd Volumes
{
    public static VolumeManager instance;

    [SerializeField] private AudioMixer Mixer;
    
    public const string MASTER_KEY = "masterVolume";
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";
    public const string TXT_KEY = "txtSpeed";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadVolume();
    }

    private void LoadVolume() //Volume gespeichert in VolumeSettings.cs
    {
        float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        float txtSpeed = PlayerPrefs.GetFloat(TXT_KEY, 0.0315f);

        Mixer.SetFloat(VolumeSettings.MIXER_MASTER, Mathf.Log10(masterVolume) * 20);
        Mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        Mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
        GameManager.instance.typingSpeed = txtSpeed;
    }
}
