using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CardIngameSlot : MonoBehaviour, IDropHandler
{
    //Verantwortlich für das Kontrollieren und Platzieren der Karten

    public string slotPosition;
    public CardIngameSlot enemyInfantryLine;
    public CardIngameSlot enemyArtilleryLine;
    public CardManager currentCard;
    
    [HideInInspector]public DragDrop dragDrop;
    [HideInInspector]public BattleSystem battleSystem;
    [HideInInspector]public PlayerManager playerManager;

    private void Start()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && battleSystem.state == BattleState.PLAYERTURN)
        {
            currentCard = eventData.pointerDrag.GetComponentInParent<CardManager>();
            dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();

            if (currentCard.GetComponent<CardDisplay>().card.position == slotPosition)
            {
                if (currentCard.cardCommandPowerCost <= playerManager.currentCommandPower)
                {
                    currentCard.currentCardMode = CardMode.INPLAY;
                    dragDrop.foundSlot = true;
                    currentCard.cardIngameSlot = this;
                    currentCard.CardPlayed();
                    eventData.pointerDrag.GetComponent<DragDrop>().rectTransform.position =
                        GetComponent<RectTransform>().position;
                }
                else
                {
                    Debug.LogWarning("Zu wenig Command Power");
                    //TODO Errorsound / Einblendung / Pochen der Commandpower Animation
                }
            }
            else
            {
                Debug.LogWarning("Falsche Position");
                //TODO Errorsound / Einblendung / Pochen der CardPosition Animation
            }
        }
    }

    public void EnemyCardPlacedOnThisSlot(CardManager cardToPlaceOnSlot)
    {
        currentCard = cardToPlaceOnSlot;
        currentCard.currentCardMode = CardMode.INPLAY;
        currentCard.cardIngameSlot = this;
        currentCard.CardPlayed();
        
        currentCard.GetComponentInChildren<DragDrop>().rectTransform.position = GetComponent<RectTransform>().position;
        currentCard.GetComponentInChildren<DragDrop>().GameObject().SetActive(false);
        
        
    }
}
