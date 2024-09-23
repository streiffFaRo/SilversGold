using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Button = UnityEngine.UI.Button;


public class CardManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Verantwortlich für Karteninformation in Hand und im Kampf

    [Header("General")]
    public Owner owner;

    [Header("CardModes")] 
    public CardMode currentCardMode = CardMode.INDECK;
    public GameObject handCard;
    public GameObject inGameCard;
    
    public int handIndex;
    [HideInInspector]public int cardCommandPowerCost;

    [Header("CardInPlayInformation")]
    public bool cardActed;
    [HideInInspector]public CardIngameSlot cardIngameSlot;

    [Header("Buttons")]
    public Button attackButton;
    public Button retreatButton;

    //Private Scripts
    private DeckManager deckManager;
    private BattleSystem battleSystem;
    private PlayerManager playerManager;
    private EnemyManager enemyManager;
    private CardDisplay cardDisplay;
    private RecruitManager recruitManager;
    private PresentDeck presentDeck;
    [HideInInspector]public Card cardStats;
    
    //Private Variablen
    private int currentHealth;
    private float hoverTimer = 0;
    private bool isHovering;
    

    private void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        battleSystem = FindObjectOfType<BattleSystem>();
        playerManager = FindObjectOfType<PlayerManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
        cardStats = GetComponent<CardDisplay>().card;
        cardDisplay = GetComponent<CardDisplay>();
        recruitManager = FindObjectOfType<RecruitManager>();
        presentDeck = FindObjectOfType<PresentDeck>();
        
        cardCommandPowerCost = cardStats.cost;
        currentHealth = cardStats.defense;
        hoverTimer = 0;
    }

    private void Update()
    {
        if (isHovering)
        {
            hoverTimer += Time.deltaTime;
            
            if (hoverTimer >= 0.5f)
            {
                handCard.SetActive(true);
                handCard.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        }
    }

    public void CardPlayed()
    {
        if (owner == Owner.PLAYER)
        {
            deckManager.availableCardSlots[handIndex] = true;
            playerManager.UpdateCommandPower(cardCommandPowerCost);
        }
        else if (owner == Owner.ENEMY)
        {
            enemyManager.availableHandCardSlots[handIndex] = true;
            enemyManager.UpdateEnemyCommandPower(cardCommandPowerCost);
            enemyManager.cardsInHand.Remove(this);
        }
        handCard.SetActive(false);
        inGameCard.SetActive(true);
        cardActed = true;
        currentCardMode = CardMode.INPLAY;
        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentCardMode == CardMode.INPLAY && battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
        {
            if (!attackButton.gameObject.activeInHierarchy)
            {
                deckManager.SetAllOtherButtonsPassive(this);
            }
            else
            {
                SetButtonsPassive();
            }
        }
        else if (currentCardMode == CardMode.INRECRUIT)
        {
            recruitManager.CardChoosen(cardStats);
        }
        else if (currentCardMode == CardMode.TODISCARD)
        {
            presentDeck.DiscardCard(cardStats);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentCardMode == CardMode.INPLAY && battleSystem.state == BattleState.PLAYERTURN || battleSystem.state == BattleState.ENEMYTURN)
        {
            //TODO Fix wenn Spieler Hover über Attack oder Retreat Button -> Keine Vergrösserung
            isHovering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentCardMode == CardMode.INPLAY)
        {
            hoverTimer = 0;
            handCard.transform.localScale = new Vector3(1f, 1f, 1f);
            handCard.SetActive(false);
            isHovering = false;
        }
    }
    
    public void SetButtonsActive()
    {
        attackButton.gameObject.SetActive(true);
        retreatButton.gameObject.SetActive(true);
    }

    public void SetButtonsPassive()
    {
        attackButton.gameObject.SetActive(false);
        retreatButton.gameObject.SetActive(false);
    }

    public void Attack()
    {
        //TODO: Verletze Karten anzeigen, dass sie nicht MaxLeben haben
        //TODO: Sounds
        //TODO: Effects
        if (battleSystem.state == BattleState.PLAYERTURN && playerManager.currentCommandPower > 0 && owner == Owner.PLAYER)
        {
            if (cardStats.position == "I")
            {
                if (cardIngameSlot.enemyInfantryLine.currentCard != null)
                {
                    UpdateCardHealth(cardIngameSlot.enemyInfantryLine.currentCard.cardStats.attack);
                    cardIngameSlot.enemyInfantryLine.currentCard.UpdateCardHealth(cardStats.attack);
                }
                else if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
                {
                    UpdateCardHealth(cardIngameSlot.enemyArtilleryLine.currentCard.cardStats.attack);
                    cardIngameSlot.enemyArtilleryLine.currentCard.UpdateCardHealth(cardStats.attack);
                }
                else
                {
                    enemyManager.UpdateEnemyHealth(cardStats.attack, false);
                }
            }
            else if (cardStats.position == "A")
            {
                if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
                {
                    UpdateCardHealth(cardIngameSlot.enemyArtilleryLine.currentCard.cardStats.attack);
                    cardIngameSlot.enemyArtilleryLine.currentCard.UpdateCardHealth(cardStats.attack);
                }
                else
                {
                    enemyManager.UpdateEnemyHealth(cardStats.attack, false);
                }
            }
            playerManager.UpdateCommandPower(1);
            SetButtonsPassive();
            cardActed = true;
            //TODO Karte soll symbolisieren dass sie genutzt wurde
        }
        else if (battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
        {
            //Wenn obere if nicht erfüllt, hat Spieler zwingen zu wenig CP
            Debug.LogWarning("Zu wenig CommandPower");
            //TODO Info an Player
        }
        else if (battleSystem.state == BattleState.ENEMYTURN && owner == Owner.ENEMY)
        {
            if (cardStats.position == "I")
            {
                if (cardIngameSlot.enemyInfantryLine.currentCard != null)
                {
                    UpdateCardHealth(cardIngameSlot.enemyInfantryLine.currentCard.cardStats.attack);
                    cardIngameSlot.enemyInfantryLine.currentCard.UpdateCardHealth(cardStats.attack);
                }
                else if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
                {
                    UpdateCardHealth(cardIngameSlot.enemyArtilleryLine.currentCard.cardStats.attack);
                    cardIngameSlot.enemyArtilleryLine.currentCard.UpdateCardHealth(cardStats.attack);
                }
                else
                {
                    playerManager.UpdateHealth(cardStats.attack, false);
                }
            }
            else if (cardStats.position == "A")
            {
                if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
                {
                    UpdateCardHealth(cardIngameSlot.enemyArtilleryLine.currentCard.cardStats.attack);
                    cardIngameSlot.enemyArtilleryLine.currentCard.UpdateCardHealth(cardStats.attack);
                }
                else
                {
                    playerManager.UpdateHealth(cardStats.attack, false);
                }
            }
            enemyManager.UpdateEnemyCommandPower(1);
            cardActed = true;
        }
        else
        {
            Debug.LogError("Attack failed!");
        }
    }

    public void Retreat()
    {
        if (battleSystem.state == BattleState.PLAYERTURN && playerManager.currentCommandPower > 0 && owner == Owner.PLAYER)
        {
            playerManager.UpdateCommandPower(1);
            SetButtonsPassive();
            deckManager.deck.Add(this);
            HandleRetreatStats();
        }
        else if (battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
        {
            //Wenn obere if nicht erfüllt, hat Spieler zwingen zu wenig CP
            Debug.LogWarning("Zu wenig CommandPower");
            //TODO Info an Player
        }
        else if (battleSystem.state == BattleState.ENEMYTURN && owner == Owner.ENEMY)
        {
            enemyManager.UpdateEnemyCommandPower(1);
            enemyManager.deck.Add(this);
            HandleRetreatStats();
        }
        else
        {
            Debug.LogError("Retreat failed!");
        }
    }

    public void Broadside()
    {
        if (battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
        {
            if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
            { 
                cardIngameSlot.enemyArtilleryLine.currentCard.UpdateCardHealth(GameManager.instance.shipCannonLevel+1);
            }
            else
            { 
                enemyManager.UpdateEnemyHealth(GameManager.instance.shipCannonLevel+1, false);
            }
            SetButtonsPassive();
            cardActed = true;
            //TODO Karte soll symbolisieren dass sie genutzt wurde
        }
        
        else if (battleSystem.state == BattleState.ENEMYTURN && owner == Owner.ENEMY)
        {
            if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
            { 
                cardIngameSlot.enemyArtilleryLine.currentCard.UpdateCardHealth(enemyManager.enemyCannonLevel);
            }
            else
            { 
                playerManager.UpdateHealth(enemyManager.enemyCannonLevel, false);
            }
            cardActed = true;
        }
        else
        {
            Debug.LogError("Attack failed!");
        }
    }

    private void HandleRetreatStats()
    {
        gameObject.SetActive(false);
        handCard.SetActive(true);
        handCard.transform.localScale = new Vector3(1f, 1f, 1f);
        inGameCard.SetActive(false);
        currentCardMode = CardMode.INDECK;
        GetComponentInChildren<DragDrop>(true).gameObject.SetActive(true);
        GetComponentInChildren<DragDrop>().foundSlot = false;
    }

    public void UpdateCardHealth(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Death();
        }
        else if (currentHealth >= cardStats.defense)
        {
            cardDisplay.inGameDefenseText.text = cardStats.defense.ToString();
        }
        else
        {
            cardDisplay.inGameDefenseText.text = currentHealth.ToString();
        }
    }
    
    public void Death()
    {
        if (owner == Owner.PLAYER)
        {
            deckManager.discardPile.Add(this);
            GetComponentInChildren<DragDrop>(true).gameObject.SetActive(true);
            GetComponentInChildren<DragDrop>().foundSlot = false;
        }
        else if (owner == Owner.ENEMY)
        {
            enemyManager.discardPile.Add(this);
            enemyManager.SetUpEnemyUI();
        }
        
        cardIngameSlot.currentCard = null;
        gameObject.SetActive(false);
        handCard.SetActive(true);
        inGameCard.SetActive(false);
        currentCardMode = CardMode.INDECK;
        cardDisplay.SetUpCardUI();
        
    }
    
}

public enum Owner
{
    PLAYER, ENEMY
}

public enum CardMode
{
    INHAND, INPLAY, INRECRUIT, INDECK, TODISCARD
}
