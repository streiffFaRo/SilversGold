using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    [Header("SFX")]
    public AudioSource platzhalterTeller;
    public AudioSource platzhalterFlasche;
    

    public void PlayPlatzHalterTeller()
    {
        platzhalterTeller.Play();
    }
    
    public void PlayPlatzHalterFlasche()
    {
        platzhalterFlasche.Play();
    }
    

}
