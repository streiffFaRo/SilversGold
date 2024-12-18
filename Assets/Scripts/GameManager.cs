using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    //Verantwortlich für das Lagern aller Variablen über Szenen hinweg
    
    public static GameManager instance;

    [Header("General")] 
    public int currentLevel;
    public int currentTier;
    public int booty;
    public int maxCommandPower;
    public int startCommandPower;
    public int maxShipHealth;
    public int startShipHealth;
    public int deckCardLimit;
    public List<Card> playerDeck = new List<Card>();
    public List<Card> startDeck = new List<Card>();
    public bool tutorialDone;

    [Header("Ship")] 
    public int shipCannonLevel;
    public int shipCaptainLevel;
    public int shipQuartersLevel;
    public int shipHullLevel;

    [Header("Settings")]
    public bool showKeyWords = true;
    public float typingSpeed = 0.03f;
    
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
        VolumeManager.instance.GetComponent<AudioManager>().PlayPirateMusic();
    }

    public void UpdateLevel() //Lädt neue Levels und Tiers
    {
        
        currentLevel++;

        //Select Tier
        if (currentLevel >= 11)
        {
            currentTier = 4;
        }
        else if (currentLevel >= 7)
        {
            currentTier = 3;
        }
        else if (currentLevel >= 4)
        {
            currentTier = 2;
        }

        //Select Music
        Debug.Log(currentLevel);
        if (currentLevel == 11)
        {
            VolumeManager.instance.GetComponent<AudioManager>().PlaySilverMusic1();
        }
        else if (currentLevel == 5)
        {
            VolumeManager.instance.GetComponent<AudioManager>().PlayNavy1Music();
        }
        
    }

    public void SetUpForNextLevel() //Lädt Schiffsupgrades
    {
        switch (shipCaptainLevel)
        {
            case 0:
                maxCommandPower = 5;
                break;
            case 1:
                maxCommandPower = 6;
                break;
            case 2:
                maxCommandPower = 7;
                break;
            default:
                maxCommandPower = 7;
                break;
        }
        
        switch (shipHullLevel)
        {
            case 0:
                maxShipHealth = 10;
                break;
            case 1:
                maxShipHealth = 12;
                break;
            case 2:
                maxShipHealth = 15;
                break;
            default:
                maxShipHealth = 15;
                break;
        }
        
        switch (shipQuartersLevel)
        {
            case 0:
                deckCardLimit = 15;
                break;
            case 1:
                deckCardLimit = 18;
                break;
            case 2:
                deckCardLimit = 20;
                break;
            default:
                deckCardLimit = 20;
                break;
        }
        
        startCommandPower = maxCommandPower;
        startShipHealth = maxShipHealth;
    }

    public void SetUpNewGame() //Setzt neues Spiel auf, resetet alle Spieldaten
    {
        
        playerDeck.Clear();
        playerDeck = new List<Card>();
        foreach (Card card in startDeck)
        {
            playerDeck.Add(card); 
        }
        
        currentLevel = 0;
        currentTier = 1;
        booty = 50;
        maxCommandPower = 5;
        startCommandPower = 5;
        maxShipHealth = 10;
        startShipHealth = 10;
        deckCardLimit = 15;

        tutorialDone = false;

        shipCannonLevel = 0;
        shipCaptainLevel = 0;
        shipQuartersLevel = 0;
        shipHullLevel = 0;
    }
    
}
