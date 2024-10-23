using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

    [Header("Ship")] 
    public int shipCannonLevel;
    public int shipCaptainLevel;
    public int shipQuartersLevel;
    public int shipHullLevel;

    [Header("Settings")]
    public bool showKeyWords = true;
    
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

    public void UpdateLevel()
    {
        currentLevel++;

        if (currentLevel >= 7)
        {
            currentTier = 3;
        }
        else if (currentLevel >= 4)
        {
            currentTier = 2;
        }
        
    }

    public void SetUpForNextLevel()
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

    public void SetUpNewGame()
    {
        playerDeck = startDeck;
        currentLevel = 0;
        currentTier = 1;
        booty = 50;
        maxCommandPower = 5;
        startCommandPower = 5;
        maxShipHealth = 10;
        startShipHealth = 10;
        deckCardLimit = 15;

        shipCannonLevel = 0;
        shipCaptainLevel = 0;
        shipQuartersLevel = 0;
        shipHullLevel = 0;
    }
    
}
