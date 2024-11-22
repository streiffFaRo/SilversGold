using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour //Einfacher Weg um UI Sounds auf dem Audiomanager abzuspielen
{
    
    private AudioManager audioManager;
    
    private void Start()
    {
        audioManager = VolumeManager.instance.GetComponent<AudioManager>();
    }

    #region PlaySounds

    public void PlayPlatzhalterTeller()
    {
        if (audioManager != null)
        {
            audioManager.platzhalterTeller.Play();
        }
    }
    
    public void PlayPlatzhalterFlasche()
    {
        if (audioManager != null)
        {
            audioManager.platzhalterFlasche.Play();
        }
    }
    
    public void PlayPressSound()
    {
        if (audioManager != null)
        {
            audioManager.PlayButtonPressSound();
        }
    }
    
    public void PlayHoverSound()
    {
        if (audioManager != null)
        {
            audioManager.PlayButtonHoverSound();
        }
    }

    public void PlayBellSound()
    {
        if (audioManager != null)
        {
            audioManager.PLayEndTurnBellSound();
        }
    }

    #endregion

}
