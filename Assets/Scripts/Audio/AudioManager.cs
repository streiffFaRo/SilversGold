using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    [Header("Platzhalter")]
    public AudioSource platzhalterTeller;
    public AudioSource platzhalterFlasche;
    [Header("CardSounds")]
    public PlayRandomSound cardAttackSound;
    public PlayRandomSound cardHandHoverSound;
    public PlayRandomSound cardPlaySound;
    public PlayRandomSound cardDrawSound;
    public AudioSource cardDeathSound;
    public AudioSource cardRetreatSound;
    [Header("ShipSounds")]
    public PlayRandomSound shipHitSound;
    public AudioSource shipDeathSound;
    [Header("UISounds")]
    public PlayRandomSound buttonPressSound;
    [Header("Other")] 
    public PlayRandomSound cannonSound;
    public AudioSource upgradeSound;
    public AudioSource bootySound;

    //Platzhalter
    public void PlayPlatzHalterTeller()
    {
        platzhalterTeller.Play();
    }
    
    public void PlayPlatzHalterFlasche()
    {
        platzhalterFlasche.Play();
    }
    
    //CardSounds
    public void PlayCardAttackSound()
    {
        cardAttackSound.PlaySound();
    }

    public void PlayCardHandHoverSound()
    {
        cardHandHoverSound.PlaySound();
    }

    public void PlayCardPlaySound()
    {
        cardPlaySound.PlaySound();
    }

    public void PlayCardDrawSound()
    {
        cardDrawSound.PlaySound();
    }

    public void PlayCardDeathSound()
    {
        cardDeathSound.Play();
    }

    public void PlayCardRetreatSound()
    {
        cardRetreatSound.Play();
    }
    
    //ShipSounds
    public void PlayShipHitSound()
    {
        shipHitSound.PlaySound();
    }

    public void PlayShipDeathSound()
    {
        shipDeathSound.Play();
    }
    
    //UISounds
    public void PlayButtonPressSound()
    {
        buttonPressSound.PlaySound();
    }
    
    //Other
    public void PlayCannonSound()
    {
        cannonSound.PlaySound();
    }

    public void PlayUpgradeSound()
    {
        upgradeSound.Play();
    }

    public void PlayBootySound()
    {
        bootySound.Play();
    }

}
