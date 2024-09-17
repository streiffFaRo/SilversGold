using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("General")] 
    public int currentLevel;
    public int currentTier;
    public int gold;
    public int maxCommandPower;
    public int startCommandPower;
    public int maxShipHealth;
    public int startShipHealth;
    public int deckCardLimit;
    public List<Card> playerDeck = new List<Card>();

    [Header("Ship")] 
    public int shipCannonLevel;
    public int shipCaptainLevel;
    public int shipCrewLevel;
    public int shipHullLevel;
    
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
        UpdateLevel();
        //Startdeck setzten
        //Alle Stats resetten
    }

    public void UpdateLevel()
    {
        currentLevel++;
    }


    public void SetUpNewGame()
    {
        
    }
    
}
