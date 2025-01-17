using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    //Verantwortlich für Kernvariablen des Gegners und deren UI Anpassung

    [Header("UI Elements")] 
    public TextMeshProUGUI enemyCommandPowerText;
    public TextMeshProUGUI enemyHealthText;
    public TextMeshProUGUI enemyDeckText;
    public TextMeshProUGUI enemyDiscardText;
    public TextMeshProUGUI enemyCannonLevelText;
    public Color damagedHealthColor;

    [Header("Commandpower Images")] 
    public Image enemyCommandPowerImage;
    public Texture2D navyCommandPowerSprite;
    public Texture2D silverCommandPowerSprite;
    
    [Header("EnemyStats")] 
    public string enemyName; //Name wird im Inspektior gestzt
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
    public DamageCounterFolder damageCounterFolder;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance; //für einfacheren Zugriff
        
        SetCommandPowerImage();
        ConvertEnemyData();
        InitiateDeck();
        MarkCardsAsEnemy();
        SetUpEnemyHealth();
        SetUpEnemyCommandPower();
        UpdateEnemyUI();
        
        enemyCannonLevelText.text = enemyCannonLevel.ToString();

        currentFatigueDamage = 1;
    }

    #region SetUp

    private void SetCommandPowerImage()
    {
        if (gameManager.currentLevel >= 11)
        {
            Sprite sprite = Sprite.Create(silverCommandPowerSprite, new Rect(0,0, silverCommandPowerSprite.width, silverCommandPowerSprite.height),new Vector2(0.5f, 0.5f));

            enemyCommandPowerImage.sprite = sprite;
        }
        else if (gameManager.currentLevel >= 5)
        {
            Sprite sprite = Sprite.Create(navyCommandPowerSprite, new Rect(0,0, navyCommandPowerSprite.width, navyCommandPowerSprite.height),new Vector2(0.5f, 0.5f));

            enemyCommandPowerImage.sprite = sprite;
        }
        
        
    }

    public void ConvertEnemyData() //Lädt Gegnerdaten für momentanes Level
    {
        enemyName = enemyData[gameManager.currentLevel - 1].enemyTitle;
        enemyMaxHealth = enemyData[gameManager.currentLevel - 1].health;
        enemyMaxCommandPower = enemyData[gameManager.currentLevel - 1].commandPower;
        enemyMaxHealth = enemyData[gameManager.currentLevel - 1].health;
        enemyCannonLevel = enemyData[gameManager.currentLevel - 1].cannonLevel;
        strategy = enemyData[gameManager.currentLevel - 1].strategy;
        deckToPrepare = enemyData[gameManager.currentLevel - 1].deckToPrepare;
    }

    public void InitiateDeck() //Lädt Gegnerdeck
    {
        foreach (Card card in deckToPrepare)
        {
            GameObject currentCardPrefab = Instantiate(displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, enemyDeckHolder.transform);
            currentCardPrefab.GetComponent<CardDisplay>().card = card;
            currentCardPrefab.SetActive(false);
            deck.Add(currentCardPrefab.GetComponent<CardManager>());
        }
    }

    public void MarkCardsAsEnemy() //Verändet die Karten so, dass der Gegner sie nutzen kann
    {
        foreach (CardManager cardToMark in deck)
        {
            cardToMark.owner = Owner.ENEMY;
            cardToMark.GetComponent<CanvasGroup>().blocksRaycasts = false;
            cardToMark.cardBG.SetActive(true);
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
        enemyCommandPowerText.text = enemyCurrentCommandPower.ToString();
        commandPowerBonus = 0;
    }

    #endregion

    #region Update Variablen

    public void UpdateEnemyCommandPower(int amount)
    {
        enemyCurrentCommandPower -= amount;
        enemyCommandPowerText.text = enemyCurrentCommandPower.ToString();
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
            VolumeManager.instance.GetComponent<AudioManager>().PlayUpgradeSound();
        }
        else
        {
            enemyCurrentHealth -= amount;
            VolumeManager.instance.GetComponent<AudioManager>().PlayShipHitSound();
        }
        
        if (enemyCurrentHealth <= 0)
        {
            enemyHealthText.text = enemyCurrentHealth.ToString();
            enemyHealthText.color = damagedHealthColor;
            if (battleSystem.state == BattleState.PLAYERTURN || battleSystem.state == BattleState.ENEMYTURN)
            {
                battleSystem.PlayerWon();
            }
        }
        else if (enemyCurrentHealth >= enemyMaxHealth)
        {
            enemyCurrentHealth = enemyMaxHealth;
            enemyHealthText.text = enemyMaxHealth.ToString();
            enemyHealthText.color = Color.white;
        }
        else
        {
            enemyHealthText.text = enemyCurrentHealth.ToString();
            enemyHealthText.color = damagedHealthColor;
        }
    }

    #endregion

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
                if (randCard.currentCardMode == CardMode.INDECK)
                {
                    for (int i = 0; i < availableHandCardSlots.Length; i++)
                    {
                        if (availableHandCardSlots[i])
                        {
                            randCard.gameObject.SetActive(true);
                            randCard.handIndex = i;
                    
                            randCard.transform.position = handCardSlots[i].position;
                            randCard.currentCardMode = CardMode.INHAND;
                            randCard.cardBG.SetActive(true);
                        
                            VolumeManager.instance.GetComponent<AudioManager>().PlayCardDrawSound();
                            availableHandCardSlots[i] = false;
                            deck.Remove(randCard);
                            cardsInHand.Add(randCard);
                            enemyDeckText.text = deck.Count.ToString();
                            return;
                        }
                    }
                }
                else
                {
                    DrawCards();
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
        damageCounterFolder.SpawnDamageCounter(enemyHealthText.rectTransform.position+ new Vector3(75,75,0), currentFatigueDamage);
        currentFatigueDamage++;
    }

    public void BuyCard()
    {
        DrawCards();
        UpdateEnemyCommandPower(2);
    }

    public IEnumerator DoEnemyStuff() //Gegner überlegt und Führt Aktionen in überschaubarem Tempo aus
    {
        yield return new WaitForSeconds(1f);
        enemyAnalysis.AnalysePossibleActions();
        yield return new WaitForSeconds(1f);
        enemyAnalysis.InitiateBestPlay();
    }

    public IEnumerator EndTurn() //Beendet Gegner Zug
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(battleSystem.PlayerTurn());
    }
}
