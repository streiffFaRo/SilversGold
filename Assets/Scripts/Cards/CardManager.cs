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

    private void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (hasBeenPlayed == false)
        {
            transform.position += Vector3.up * 50;
            hasBeenPlayed = true;
            deckManager.availableCardSlots[handIndex] = true;
            Invoke("MoveToDiscardPile", 2f);
        }
    }

    void MoveToDiscardPile()
    {
        deckManager.discardPile.Add(this);
        gameObject.SetActive(false);
    }
}
