using System;
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
    public PlayRandomSound buttonHoverSound;
    
    [Header("Music")] 
    public AudioSource pirateMusic;
    public AudioSource navyMusic1;
    public AudioSource navyMusic2;
    public AudioSource silverMusic1;
    public AudioSource silverMusic2;
    public AudioSource silverMusic3;

    public AudioSource currentMusic;
    
    [Header("Other")] 
    public PlayRandomSound cannonSound;
    public AudioSource upgradeSound;
    public AudioSource bootySound;
    public AudioSource endTurnBellSound;
    public PlayRandomSound denySound;

    private void Start()
    {
        currentMusic = null;
    }

    #region Platzhalter
    
    public void PlayPlatzHalterTeller()
    {
        platzhalterTeller.Play();
    }
    
    public void PlayPlatzHalterFlasche()
    {
        platzhalterFlasche.Play();
    }

    #endregion

    #region CardSounds
    
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

    #endregion

    #region ShipSounds
    
    public void PlayShipHitSound()
    {
        shipHitSound.PlaySound();
    }

    public void PlayShipDeathSound()
    {
        shipDeathSound.Play();
    }

    #endregion

    #region UISounds
    
    public void PlayButtonPressSound()
    {
        buttonPressSound.PlaySound();
    }
    
    public void PlayButtonHoverSound()
    {
        buttonHoverSound.PlaySound();
    }

    #endregion

    #region Music

    public void PlayPirateMusic()
    {
        if (currentMusic != null)
        {
            currentMusic.Stop();
        }
        currentMusic = pirateMusic;
        pirateMusic.Play();
    }

    public void PlayNavy1Music()
    {
        pirateMusic.Stop();
        navyMusic1.Play();
    }
    public void PlayNavy2Music()
    {
        if (currentMusic != null)
        {
            currentMusic.Stop();
        }
        currentMusic = navyMusic2;
        navyMusic2.Play();
    }

    public void PlaySilverMusic1()
    {
        pirateMusic.Stop();
        navyMusic1.Stop();
        silverMusic1.Play();
    }

    public void PlaySilverMusic2()
    {
        if (currentMusic != null)
        {
            currentMusic.Stop();
        }
        currentMusic = silverMusic2;
        silverMusic2.Play();
    }

    public void PlaySilverMusic3()
    {
        if (currentMusic != null)
        {
            currentMusic.Stop();
        }
        currentMusic = silverMusic3;
        silverMusic3.Play();
    }

    #endregion

    #region Other
    
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
    
    public void PLayEndTurnBellSound()
    {
        endTurnBellSound.Play();
    }

    public void PlayDenySound()
    {
        denySound.PlaySound();
    }
    

    #endregion

}
