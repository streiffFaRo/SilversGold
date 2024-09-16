using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    public int enemyMaxHealth;
    public int enemyCurrentHealth;
    public Strategies strategy;
    public List<Card> deckToPrepare = new List<Card>();

    [Header("EnemyDeckManagement")] 
    public GameObject displayCardPrefab;
    public List<CardManager> deck = new List<CardManager>();
    public List<CardManager> discardPile = new List<CardManager>();
    public Transform[] cardSlots;
    public bool[] availableCardSlots;
    public List<CardManager> cardsInHand = new List<CardManager>();
    
    [Header("EnemyData")] 
    public List<EnemyData> enemyData = new List<EnemyData>();

    public EnemyStrategy enemyStrategy;
    public EnemyAnalysis enemyAnalysis;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        
        ConvertEnemyData();
        InitilizeDeck();
        MarkCardsAsEnemy();
        SetUpEnemyHealth();
        SetUpEnemyCommandPower();
        SetUpEnemyUI();
    }

    public void ConvertEnemyData()
    {
        enemyName = enemyData[gameManager.currentLevel - 1].enemyTitle;
        enemyMaxHealth = enemyData[gameManager.currentLevel - 1].health;
        enemyMaxCommandPower = enemyData[gameManager.currentLevel - 1].commandPower;
        enemyMaxHealth = enemyData[gameManager.currentLevel - 1].health;
        strategy = enemyData[gameManager.currentLevel - 1].strategies;
        deckToPrepare = enemyData[gameManager.currentLevel - 1].deckToPrepare;
    }

    public void InitilizeDeck()
    {
        foreach (Card card in deckToPrepare)
        {
            GameObject currentCardPrefab = Instantiate(displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
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
            cardToMark.GetComponent<CanvasGroup>().interactable = false;
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
        enemyCommandPowerText.text = enemyMaxCommandPower.ToString();
    }

    public void SetUpEnemyUI()
    {
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

            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;
                    
                    randCard.transform.position = cardSlots[i].position;
                    randCard.hasBeenPlayed = false;
                    
                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    cardsInHand.Add(randCard);
                    enemyDeckText.text = deck.Count.ToString();
                    return;
                }
            }
        }
    }

    public IEnumerator DoEnemyStuff()
    {
        Debug.Log("Ich denke über böse Sachen nach");
        yield return new WaitForSeconds(1f);
        enemyStrategy.PlayRdmCard();
        yield return new WaitForSeconds(1f);
        enemyStrategy.LetAllEnemysAttack();
        yield return new WaitForSeconds(1f);
        Debug.Log("Ich habe böse Sachen gemacht");
        FindObjectOfType<BattleSystem>().PlayerTurn();
        
    }
}
