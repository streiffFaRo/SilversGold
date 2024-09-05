using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public bool hasBeenPlayed;
    public int handIndex;

    private DeckManager deckManager;

    private void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
    }

    private void OnMouseEnter()
    {
        Debug.Log("VAR");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("VAR");
    }

    private void OnMouseDown()
    {
        Debug.Log("XXX");
        if (hasBeenPlayed == false)
        {
            transform.position += Vector3.up * 5;
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
