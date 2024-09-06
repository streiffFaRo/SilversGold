using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour, IPointerClickHandler
{
    public bool hasBeenPlayed;
    public int handIndex;

    private DeckManager deckManager;
    private int cardCommandPowerCost;

    private void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        cardCommandPowerCost = GetComponent<CardDisplay>().card.cost;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (hasBeenPlayed == false)
        {
            if (deckManager.currentCommandPower >= cardCommandPowerCost)
            {
                transform.position += Vector3.up * 400;
                hasBeenPlayed = true;
                deckManager.availableCardSlots[handIndex] = true;
                deckManager.UpdateCommandPower(cardCommandPowerCost);
                Invoke("MoveToDiscardPile", 2f);
            }
            else
            {
                Debug.LogWarning("Zu wenig Comman Power!");
            }
        }
    }

    void MoveToDiscardPile()
    {
        deckManager.discardPile.Add(this);
        gameObject.SetActive(false);
    }
}
