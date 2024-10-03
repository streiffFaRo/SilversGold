using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    //Verantwortlich für Deck, Ablagestapel, ziehen von Karten und managen der Handkarten des Spielers

    [Header("General")]
    public GameObject displayCardPrefab;
    public GameObject deckHolder; //Alle Karten des Spielerdecks werden seine Kinder für die Ordnung
    public List<Card> deckToPrepare = new List<Card>();
    public List<CardManager> deck = new List<CardManager>();
    public List<CardManager> discardPile = new List<CardManager>();
    public List<CardManager> cardsInHand = new List<CardManager>();
    public int currentFatigueDamage = 1;
    
    [Header("Display")]
    public GameObject displayStand;
    public CardDisplay displayUI;
    
    [HideInInspector]public CardManager[] allPresentCards;
    public Transform[] cardSlots;
    public bool[] availableCardSlots;
    
    [Header("Scripts")]
    public BattleSystem battleSystem;
    public PlayerManager playerManager;

    private void Awake()
    {
        InitilizeDeck();
    }

    private void Start()
    {
        GameObject displayObj = Instantiate(displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, displayStand.transform);
        displayObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        displayUI = displayStand.GetComponentInChildren<CardDisplay>();
        currentFatigueDamage = 1;
    }

    private void InitilizeDeck()
    {
        foreach (Card card in deckToPrepare)
        {
            GameObject currentCardPrefab = Instantiate(displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, deckHolder.transform);
            currentCardPrefab.GetComponent<CardDisplay>().card = card;
            currentCardPrefab.SetActive(false);
            deck.Add(currentCardPrefab.GetComponent<CardManager>());
        }
    }

    private void Update()
    {
        //TODO beide nachfolgenden Zeilen neu eingliedern, ausserhalb von Update
        playerManager.deckSizeText.text = deck.Count.ToString();
        playerManager.discardPileText.text = discardPile.Count.ToString();
    }
    
    public void DrawCards()
    {
        if (deck.Count >= 1)
        {
            CardManager randCard = deck[Random.Range(0, deck.Count)];

            if (cardsInHand.Count <= 5)
            {
                for (int i = 0; i < availableCardSlots.Length; i++)
                {
                    if (availableCardSlots[i])
                    {
                        randCard.gameObject.SetActive(true);
                        randCard.handIndex = i;
                    
                        randCard.transform.position = cardSlots[i].position;
                        randCard.currentCardMode = CardMode.INHAND;
                    
                        availableCardSlots[i] = false;
                        deck.Remove(randCard);
                        cardsInHand.Add(randCard);
                        return;
                    }
                }
            }
            else
            {
                BurnTopDeckCard(randCard);
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
        //TODO Animation & Sound
    }
    
    public void Fatigue()
    {
        playerManager.UpdateHealth(currentFatigueDamage, false);
        Debug.LogWarning("You fatigued for " + currentFatigueDamage);
        //TODO Animation & Sound
        currentFatigueDamage++;
    }

    public void ButtonDrawsCards()
    {
        if (playerManager.currentCommandPower >= 2 && cardsInHand.Count < 5)
        {
            playerManager.UpdateCommandPower(2);
            DrawCards();
        }
        else
        {
            Debug.LogWarning("Zu wenig CommandPower / Volle Hand, Karte kann nicht gezogen werden!");
        }
    }

    public void Shuffle()
    {
        if (discardPile.Count >=1 && battleSystem.state == BattleState.PLAYERTURN)
        {
            playerManager.RefreshCommandPower();
            foreach (CardManager cM in discardPile)
            {
                deck.Add(cM);
            }
            discardPile.Clear();
        }
    }

    public void Broadside()
    {
        if (battleSystem.state == BattleState.PLAYERTURN && playerManager.currentCommandPower >= 2)
        {
            foreach (CardManager card in FindObjectsOfType<CardManager>())
            { 
                if (card.owner == Owner.PLAYER && card.currentCardMode == CardMode.INPLAY && !card.cardActed && card.cardStats.isCannoneer) 
                {
                    card.Broadside();
                }
            }
            playerManager.UpdateCommandPower(2);
        }
    }

    public void EndTurn()
    {
        if (battleSystem.state == BattleState.PLAYERTURN)
        {
            battleSystem.EnemyTurn();
        }
    }

    public void ShowDisplayCard(CardManager cardManager)
    {
        displayStand.SetActive(true);
        displayUI.card = cardManager.cardStats;
        displayUI.SetUpCardUI();
    }

    public void HideDisplayCard()
    {
        displayStand.SetActive(false);
    }

    public void SetAllOtherButtonsPassive(CardManager targetCardManager)
    {
        if (!targetCardManager.cardActed)
        {
            allPresentCards = FindObjectsOfType<CardManager>();

            foreach (CardManager card in allPresentCards)
            {
                for (int i = 0; i < allPresentCards.Length; i++)
                {
                    if (card.currentCardMode == CardMode.INPLAY && card.owner == Owner.PLAYER)
                    {
                        card.SetButtonsPassive();
                        
                    }
                }
            }
            targetCardManager.SetButtonsActive();
        }
        else
        {
            Debug.LogWarning("Karte hat schon gehandelt");
            //TODO Info an Player
        }
        
    }
}
