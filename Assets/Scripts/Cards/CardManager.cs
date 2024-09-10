using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class CardManager : MonoBehaviour, IPointerClickHandler
{
    [Header("CardInHandInformation")]
    public bool hasBeenPlayed;
    public int handIndex;

    [Header("CardModes")]
    public GameObject handCard;
    public GameObject inGameCard;

    [Header("Buttons")]
    public Button attackButton;
    public Button retreatButton;

    //Private Variablen
    private DeckManager deckManager;
    private Card cardStats;
    private BattleSystem battleSystem;
    private int cardCommandPowerCost;

    [HideInInspector] public bool foundSlot = false;

    private void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        cardStats = GetComponent<CardDisplay>().card;
        cardCommandPowerCost = cardStats.cost;
        battleSystem = FindObjectOfType<BattleSystem>();
    }
    
    public void CardPlayed()
    {
        handCard.SetActive(false);
        inGameCard.SetActive(true);
        hasBeenPlayed = true;
        deckManager.availableCardSlots[handIndex] = true;
        deckManager.UpdateCommandPower(cardCommandPowerCost);
        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (foundSlot && battleSystem.state == BattleState.PLAYERTURN)
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
        if (battleSystem.state == BattleState.PLAYERTURN && deckManager.currentCommandPower > 0)
        {
            deckManager.UpdateCommandPower(1);
            SetButtonsPassive();
            FindObjectOfType<EnemyManager>().UpdateEnemyHealth(cardStats.attack);
            //Button soll nicht nochmal aufgerufen werden können
            //Karte soll symbolisieren dass sie genutzt wurde
            Debug.Log("Attack!");
        }
        else if (battleSystem.state == BattleState.PLAYERTURN)
        {
            Debug.Log("Zu wenig CommandPower");
        }
    }

    public void Retreat()
    {
        if (battleSystem.state == BattleState.PLAYERTURN && deckManager.currentCommandPower > 0)
        {
            deckManager.UpdateCommandPower(1);
            SetButtonsPassive();
            deckManager.discardPile.Add(this);
            gameObject.SetActive(false);
            handCard.SetActive(true);
            inGameCard.SetActive(false);
            hasBeenPlayed = false;
            GetComponentInChildren<DragDrop>(true).gameObject.SetActive(true);
            foundSlot = false;
            GetComponentInChildren<DragDrop>().foundSlot = false;
        }
        else if (battleSystem.state == BattleState.PLAYERTURN)
        {
            Debug.Log("Zu wenig CommandPower");
        }
    }

    public void Death()
    {
        
    }
    
}
