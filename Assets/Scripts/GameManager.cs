using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("General")] 
    public int currentLevel;
    public int gold;
    public int maxCommandPower;
    public int currentCommandPower;
    public int maxShipHealth;
    public int currentShipHealth;
    public int deckCardLimit;
    //Deck

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
    
    public void SetUpNewGame()
    {
        
    }
    
}
