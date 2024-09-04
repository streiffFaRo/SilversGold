using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    public AudioSource[] sounds;
    public bool pitch;
    public float pitchAmount;
    
    
    
    public void PlaySound()
    {
        int randomSound = Random.Range(0, sounds.Length);

        AudioSource soundToPlay = sounds[randomSound];

        if (pitch)
        {
            float randomPitch = Random.Range(1 - pitchAmount, 1 + pitchAmount);
            soundToPlay.pitch = randomPitch;
            
        }
        
        soundToPlay.Play();
        
    }
}
