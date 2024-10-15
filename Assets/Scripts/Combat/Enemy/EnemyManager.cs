using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    public int commandPowerBonus;
    public int enemyMaxHealth;
    public int enemyCurrentHealth;
    public int enemyCannonLevel;
    public Strategy strategy;
    public List<Card> deckToPrepare = new List<Card>();

    [Header("EnemyDeckManagement")]
    public GameObject displayCardPrefab; //Prefab aus dem Karten für das Deck gemacht werden
    public GameObject enemyDeckHolder; //Obj unter dem Deck des Gegners generiert und sortiert ist
    public List<CardManager> deck = new List<CardManager>();
    public List<CardManager> discardPile = new List<CardManager>();
    public Transform[] handCardSlots; //Slots für Handkarten
    public bool[] availableHandCardSlots; //Verfügbare Slots für Handkarten
    public List<CardManager> cardsInHand = new List<CardManager>(); //Karten in der Hande des Enemy
    public int currentFatigueDamage = 1;
    
    [Header("EnemyData")] 
    public List<EnemyData> enemyData = new List<EnemyData>();

    [Header("Scripts")]
    public EnemyActionExecuter enemyActionExecuter;
    public EnemyAnalysis enemyAnalysis;
    public BattleSystem battleSystem;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        
        ConvertEnemyData();
        InitiateDeck();
        MarkCardsAsEnemy();
        SetUpEnemyHealth();
        SetUpEnemyCommandPower();
        UpdateEnemyUI();

        currentFatigueDamage = 1;
    }

    public void ConvertEnemyData()
    {
        enemyName = enemyData[gameManager.currentLevel - 1].enemyTitle;
        enemyMaxHealth = enemyData[gameManager.currentLevel - 1].health;
        enemyMaxCommandPower = enemyData[gameManager.currentLevel - 1].commandPower;
        enemyMaxHealth = enemyData[gameManager.currentLevel - 1].health;
        enemyCannonLevel = enemyData[gameManager.currentLevel - 1].cannonLevel;
        strategy = enemyData[gameManager.currentLevel - 1].strategy;
        deckToPrepare = enemyData[gameManager.currentLevel - 1].deckToPrepare;
    }

    public void InitiateDeck()
    {
        foreach (Card card in deckToPrepare)
        {
            GameObject currentCardPrefab = Instantiate(displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, enemyDeckHolder.transform);
            currentCardPrefab.GetComponent<CardDisplay>().card = card;
            currentCardPrefab.SetActive(false);
            deck.Add(currentCardPrefab.GetComponent<CardManager>());
        }
    }

    public void MarkCardsAsEnemy()
    {
        foreach (CardManager cardToMark in deck)
        {
            cardToMark.owner = Owner.ENEMY;
            cardToMark.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void SetUpEnemyHealth()
    {
        enemyCurrentHealth = enemyMaxHealth;
        enemyHealthText.text = enemyMaxHealth.ToString();
    }

    public void SetUpEnemyCommandPower()
    {
        enemyCurrentCommandPower = enemyMaxCommandPower;
        enemyCurrentCommandPower += commandPowerBonus;
        enemyCommandPowerText.text = enemyMaxCommandPower.ToString();
        commandPowerBonus = 0;
    }

    public void UpdateEnemyUI()
    {
        enemyDeckText.text = deck.Count.ToString();
        enemyDiscardText.text = discardPile.Count.ToString();
    }

    public void UpdateEnemyHealth(int amount, bool positiveNumber)
    {
        
        if (positiveNumber)
        {
            enemyCurrentHealth += amount;
        }
        else
        {
            enemyCurrentHealth -= amount;
            VolumeManager.instance.GetComponent<AudioManager>().PlayShipHitSound();
        }
        
        if (enemyCurrentHealth <= 0)
        {
            enemyHealthText.text = enemyCurrentHealth.ToString();
            battleSystem.PlayerWon();
        }
        else if (enemyCurrentHealth >= enemyMaxHealth)
        {
            enemyHealthText.text = enemyMaxHealth.ToString();
        }
        else
        {
            enemyHealthText.text = enemyCurrentHealth.ToString();
        }
    }

    public void UpdateEnemyCommandPower(int amount)
    {
        enemyCurrentCommandPower -= amount;
        enemyCommandPowerText.text = enemyCurrentCommandPower.ToString();
    }

    public void StartNewEnemyTurn()
    {
        enemyCurrentCommandPower = enemyMaxCommandPower;
        ResetCardsWhoActed();
        SetUpEnemyCommandPower();
        StartCoroutine(DoEnemyStuff());
        DrawCards();

    }
    
    public void ResetCardsWhoActed()
    {
        CardManager[] allCardsInPlay = FindObjectsOfType<CardManager>();

        foreach (CardManager cardToReset in allCardsInPlay)
        {
            if (cardToReset.cardActed && cardToReset.owner == Owner.ENEMY)
            {
                cardToReset.cardActed = false;
            }
        }
    }
    
    public void DrawCards()
    {
        if (deck.Count >= 1)
        {
            CardManager randCard = deck[Random.Range(0, deck.Count)];

            if (cardsInHand.Count >= 5)
            {
                BurnTopDeckCard(randCard);
            }
            else
            {
                for (int i = 0; i < availableHandCardSlots.Length; i++)
                {
                    if (availableHandCardSlots[i])
                    {
                        randCard.gameObject.SetActive(true);
                        randCard.handIndex = i;
                    
                        randCard.transform.position = handCardSlots[i].position;
                        randCard.currentCardMode = CardMode.INHAND;
                        
                        VolumeManager.instance.GetComponent<AudioManager>().PlayCardDrawSound();
                        availableHandCardSlots[i] = false;
                        deck.Remove(randCard);
                        cardsInHand.Add(randCard);
                        enemyDeckText.text = deck.Count.ToString();
                        return;
                    }
                }
            }
        }
        else
        {
            Fatigue();
        }
    }
    
    public void BurnTopDeckCard(CardManager cardToBurn)
    {
        deck.Remove(cardToBurn);
        discardPile.Add(cardToBurn);
        UpdateEnemyUI();
        //TODO Animation & Sound
    }
    
    public void Fatigue()
    {
        UpdateEnemyHealth(currentFatigueDamage, false);
        Debug.LogWarning("You fatigued for " + currentFatigueDamage);
        //TODO Animation & Sound
        currentFatigueDamage++;
    }

    public void BuyCard()
    {
        DrawCards();
        UpdateEnemyCommandPower(2);
    }

    public IEnumerator DoEnemyStuff()
    {
        yield return new WaitForSeconds(1f);
        enemyAnalysis.AnalysePossibleActions();
        yield return new WaitForSeconds(1f);
        enemyAnalysis.InitiateBestPlay();
    }

    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(battleSystem.PlayerTurn());
    }
}
