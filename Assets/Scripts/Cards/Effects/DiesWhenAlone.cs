using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiesWhenAlone : MonoBehaviour 
{
    //Verantwortlich für Karten die sterben sollen, wenn sie die letzen in der Inf Reihe sind
    
    //Effekt nur für Infanteriekarten des Spielers möglich!
    
    private CardIngameSlot[] slots;
    private CardManager cardManager;
    
    private void Start()
    {
        slots = FindObjectsOfType<CardIngameSlot>();
        cardManager = GetComponent<CardManager>();
    }

    public void CheckIfAlone() //Überprüft ob die Karte alleine in der Inf Reihe ist - Prüfung geht vom CardManager aus
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
    }

    public IEnumerator killCard() //Tötet Karte mit kleiner Verzögerung
    {
        yield return new WaitForSeconds(0.5f);
        //TODO Animation
        //TODO Scream Sound
        cardManager.Death();
    }
}
