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
    public List<Card> deckToPrepare = new List<Card>();
    public List<CardManager> deck = new List<CardManager>();
    public List<CardManager> discardPile = new List<CardManager>();
    
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

    private void InitilizeDeck()
    {
        foreach (Card card in deckToPrepare)
        {
            GameObject currentCardPrefab = Instantiate(displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
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
                    return;
                }
            }
        }
    }
    
    public void Shuffle()
    {
        if (discardPile.Count >=1 && battleSystem.state == BattleState.PLAYERTURN)
        {
            playerManager.SetUpCommandPower();
            foreach (CardManager cM in discardPile)
            {
                deck.Add(cM);
            }
            discardPile.Clear();
        }
    }

    public void EndTurn()
    {
        if (battleSystem.state == BattleState.PLAYERTURN)
        {
            battleSystem.EnemyTurn();
        }
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
                    if (card.foundSlot && card.owner == Owner.PLAYER)
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
