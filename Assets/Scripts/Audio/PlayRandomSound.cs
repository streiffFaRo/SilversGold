using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    
    public AudioSource[] sounds;
    public bool pitch;
    public float pitchAmount;
    
    
    public void PlaySound() //Spielt zufälligen Sound aus einem Array, auch mit zufälligem Pitch
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
