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
    public int commandPowerBonus;
    public int maxHealth;
    public int currentHealth;

    [Header("UI Elements")]
    public TextMeshProUGUI deckSizeText;
    public TextMeshProUGUI discardPileText;
    public TextMeshProUGUI commandPowerText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI canonLevelText;

    [Header("Scripts")]
    public BattleSystem battleSystem;
    public DeckManager DeckManager;

    private void Start()
    {
        maxCommandPower = GameManager.instance.startCommandPower;
        SetUpHealth();
        RefreshCommandPower();
    }
    
    public void SetUpHealth()
    {
        maxHealth = GameManager.instance.maxShipHealth;
        currentHealth = GameManager.instance.startShipHealth;
        healthText.text = currentHealth.ToString();
        
        int currentCannonLevel = GameManager.instance.shipCannonLevel + 1;
        canonLevelText.text = currentCannonLevel.ToString();
    }
    
    public void RefreshCommandPower()
    {
        currentCommandPower = maxCommandPower;
        currentCommandPower += commandPowerBonus;
        commandPowerText.text = currentCommandPower.ToString();
        commandPowerBonus = 0;
    }  
    
    public void StartNewTurn()
    {
        RefreshCommandPower();
        DeckManager.DrawCards();
        ResetCardsWhoActed();
    }

    public void ResetCardsWhoActed()
    {
        CardManager[] allCardsInPlay = FindObjectsOfType<CardManager>();

        foreach (CardManager cardToReset in allCardsInPlay)
        {
            if (cardToReset.cardActed && cardToReset.owner == Owner.PLAYER)
            {
                cardToReset.cardActed = false;
            }
        }
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
            VolumeManager.instance.GetComponent<AudioManager>().PlayUpgradeSound();
        }
        else
        {
            currentHealth -= amount;
            VolumeManager.instance.GetComponent<AudioManager>().PlayShipHitSound();
        }
        
        if (currentHealth <= 0)
        {
            healthText.text = currentHealth.ToString();
            healthText.color = Color.red;
            battleSystem.GameOver();
        }
        else if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            healthText.text = maxHealth.ToString();
            healthText.color = Color.white;
        }
        else
        {
            healthText.text = currentHealth.ToString();
            healthText.color = Color.red;
        }
    }
}
