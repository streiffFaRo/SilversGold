using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Verantwortlich für Kernvariablen des Spielers und deren UI Anpassung
    
    [Header("PlayerStats")]
    public int maxCommandPower;
    public int currentCommandPower;
    public int maxHealth;
    public int currentHealth;

    [Header("UI Elements")]
    public TextMeshProUGUI deckSizeText;
    public TextMeshProUGUI discardPileText;
    public TextMeshProUGUI commandPowerText;
    public TextMeshProUGUI healthText;

    [Header("Scripts")]
    public BattleSystem battleSystem;
    public DeckManager DeckManager;

    private void Start()
    {
        SetUpCommandPower();
        SetUpHealth();
    }

    public void SetUpCommandPower()
    {
        currentCommandPower = GameManager.instance.startCommandPower;
        commandPowerText.text = currentCommandPower.ToString();
    }  
    
    public void SetUpHealth()
    {
        maxHealth = GameManager.instance.maxShipHealth;
        currentHealth = GameManager.instance.startShipHealth;
        healthText.text = currentHealth.ToString();
    }
    
    public void StartNewTurn()
    {
        SetUpCommandPower();
        DeckManager.DrawCards();
    }
    
    public void UpdateCommandPower(int commandPowerCost)
    {
        currentCommandPower -= commandPowerCost;
        commandPowerText.text = currentCommandPower.ToString();
    }
    
    public void UpdateHealth(int amount, bool positiveNumber)
    {
        if (positiveNumber)
        {
            currentHealth += amount;
        }
        else
        {
            currentHealth -= amount;
        }
        
        if (currentHealth <= 0)
        {
            battleSystem.GameOver();
        }
        else if (currentHealth >= maxHealth)
        {
            healthText.text = maxHealth.ToString();
        }
        else
        {
            healthText.text = currentHealth.ToString();
        }
        
    }
}
