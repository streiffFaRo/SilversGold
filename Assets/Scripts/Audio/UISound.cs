using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
    
    private AudioManager audioManager;
    
    private void Start()
    {
        audioManager = VolumeManager.instance.GetComponent<AudioManager>();
    }

    public void PlayPlatzhalterTeller()
    {
        if (audioManager != null)
        {
            audioManager.platzhalterTeller.Play();
        }
    }

}
