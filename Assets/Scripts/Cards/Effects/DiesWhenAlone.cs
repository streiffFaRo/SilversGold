using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiesWhenAlone : MonoBehaviour
{
    //Effekt nur für Infanteriekarten des Spielers möglich!
    
    private CardIngameSlot[] slots;
    private CardManager cardManager;
    
    private void Start()
    {
        slots = FindObjectsOfType<CardIngameSlot>();
        cardManager = GetComponent<CardManager>();
    }

    public void CheckIfAlone()
    {
        int emptySlots = 0;
        
        foreach (CardIngameSlot slot in slots)
        {
            if (slot.slotPosition == "I")
            {
                if (slot.currentCard == null)
                {
                    emptySlots++;
                }

                if (emptySlots >= 2)
                {
                    StartCoroutine(killCard());
                }
            }
        }
        emptySlots = 0;
    }

    public IEnumerator killCard()
    {
        yield return new WaitForSeconds(0.5f);
        //TODO Animation
        //TODO Scream Sound
        cardManager.Death();
    }
}
