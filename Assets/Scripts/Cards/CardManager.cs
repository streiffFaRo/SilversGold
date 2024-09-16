using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;


public class CardManager : MonoBehaviour, IPointerClickHandler
{
    //Verantwortlich für Karteninformation in Hand und im Kampf

    [Header("General")]
    public Owner owner;
    
    [Header("CardModes")]
    public GameObject handCard;
    public GameObject inGameCard;
    
    [Header("CardInHandInformation")]
    public bool hasBeenPlayed;
    public int handIndex;
    [HideInInspector]public int cardCommandPowerCost;

    //CardInPlayInformation
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
    [HideInInspector]public Card cardStats;
    
    //Private Variablen
    private int currentHealth;

    [HideInInspector] public bool foundSlot = false;
    

    private void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        battleSystem = FindObjectOfType<BattleSystem>();
        playerManager = FindObjectOfType<PlayerManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
        cardStats = GetComponent<CardDisplay>().card;
        cardDisplay = GetComponent<CardDisplay>();
        
        cardCommandPowerCost = cardStats.cost;
        currentHealth = cardStats.defense;
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
            enemyManager.availableCardSlots[handIndex] = true;
            enemyManager.UpdateEnemyCommandPower(cardCommandPowerCost);
            enemyManager.cardsInHand.Remove(this);
        }
        handCard.SetActive(false);
        inGameCard.SetActive(true);
        hasBeenPlayed = true;
        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (foundSlot && battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
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
        if (battleSystem.state == BattleState.PLAYERTURN && playerManager.currentCommandPower > 0 && owner == Owner.PLAYER)
        {
            playerManager.UpdateCommandPower(1);
            SetButtonsPassive();
            cardActed = true;
            //TODO momentan greifen Karten Gegner direkt an
            enemyManager.UpdateEnemyHealth(cardStats.attack);
            //TODO Karte soll symbolisieren dass sie genutzt wurde
            Debug.Log("Attack!");
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
            cardActed = true;
            playerManager.UpdateHealth(cardStats.attack, false);
            Debug.Log("Attack!");
        }
        else
        {
            Debug.LogWarning("Attack failed!");
        }
    }

    public void Retreat()
    {
        if (battleSystem.state == BattleState.PLAYERTURN && playerManager.currentCommandPower > 0 && owner == Owner.PLAYER)
        {
            playerManager.UpdateCommandPower(1);
            SetButtonsPassive();
            deckManager.deck.Add(this);
            gameObject.SetActive(false);
            handCard.SetActive(true);
            inGameCard.SetActive(false);
            hasBeenPlayed = false;
            GetComponentInChildren<DragDrop>(true).gameObject.SetActive(true);
            foundSlot = false;
            GetComponentInChildren<DragDrop>().foundSlot = false;
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
            gameObject.SetActive(false);
            handCard.SetActive(true);
            inGameCard.SetActive(false);
            hasBeenPlayed = false;
            foundSlot = false;
        }
        else
        {
            Debug.LogWarning("Retreat failed!");
        }
    }

    public void UpdateCardHealth(int damage)
    {
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
        }
        
        cardIngameSlot.currentCard = null;
        gameObject.SetActive(false);
        handCard.SetActive(true);
        inGameCard.SetActive(false);
        hasBeenPlayed = false;
        foundSlot = false;
        cardDisplay.SetUpCardUI();
        
    }
    
}

public enum Owner
{
    PLAYER, ENEMY
}
