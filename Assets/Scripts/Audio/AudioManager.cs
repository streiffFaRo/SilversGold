using UnityEngine;

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
    public AudioSource menuMusic;
    public AudioSource gameMusic;
    
    [Header("Other")] 
    public PlayRandomSound cannonSound;
    public AudioSource upgradeSound;
    public AudioSource bootySound;
    public AudioSource endTurnBellSound;
    public PlayRandomSound denySound;


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

    public void PlayMenuMusic()
    {
        menuMusic.Play();
    }

    public void StopMenuMusic()
    {
        menuMusic.Stop();
    }

    public void PlayGameMusic()
    {
        gameMusic.Play();
    }

    public void StopGameMusic()
    {
        gameMusic.Stop();
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
