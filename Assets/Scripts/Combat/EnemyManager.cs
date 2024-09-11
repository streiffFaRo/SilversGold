using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Verantwortlich für Kernvariablen des Gegners und deren UI Anpassung

    [Header("UI Elements")]
    public TextMeshProUGUI enemyCommandPowerText;
    public TextMeshProUGUI enemyHealthText;
    public TextMeshProUGUI enemyDeckText;
    public TextMeshProUGUI enemyDiscardText;

    [Header("EnemyStats")] 
    public string enemyName;
    public int enemyMaxCommandPower;
    public int enemyCurrentCommandPower;
    public int enemyMaxHealth;
    public int enemyCurrentHealth;
    public Strategies strategy;
    public List<CardManager> deck = new List<CardManager>();
    public List<CardManager> discardPile = new List<CardManager>();
    
    public List<EnemyData> enemyData = new List<EnemyData>();
    
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        
        ConvertEnemyData();
        SetUpEnemyStats();
        SetUpEnemyUI();
    }

    public void ConvertEnemyData()
    {
        enemyName = enemyData[gameManager.currentLevel - 1].enemyTitle;
        enemyMaxHealth = enemyData[gameManager.currentLevel - 1].health;
        enemyMaxCommandPower = enemyData[gameManager.currentLevel - 1].commandPower;
        enemyMaxHealth = enemyData[gameManager.currentLevel - 1].health;
        strategy = enemyData[gameManager.currentLevel - 1].strategies;
        deck = enemyData[gameManager.currentLevel - 1].deck;
    }

    public void SetUpEnemyStats()
    {
        enemyCurrentHealth = enemyMaxHealth;
        enemyCurrentCommandPower = enemyMaxCommandPower;
    }

    public void SetUpEnemyUI()
    {
        enemyCommandPowerText.text = enemyMaxCommandPower.ToString();
        enemyHealthText.text = enemyMaxHealth.ToString();
        enemyDeckText.text = deck.Count.ToString();
        enemyDiscardText.text = discardPile.Count.ToString();
    }

    public void UpdateEnemyHealth(int amount)
    {
        enemyCurrentHealth -= amount;
        enemyHealthText.text = enemyCurrentHealth.ToString();
    }

    public void UpdateEnemyCommandPower(int amount)
    {
        enemyCurrentCommandPower -= amount;
        enemyCommandPowerText.text = enemyCurrentCommandPower.ToString();
    }

    public void StartNewEnemyTurn()
    {
        enemyCurrentCommandPower = enemyMaxCommandPower;
        StartCoroutine(DoEnemyStuff());
        //Draw Card

    }

    public IEnumerator DoEnemyStuff()
    {
        Debug.Log("Ich denke über böse Sachen nach");
        yield return new WaitForSeconds(3f);
        Debug.Log("Ich habe böse Sachen gemacht");
        FindObjectOfType<BattleSystem>().PlayerTurn();
        
    }
}
