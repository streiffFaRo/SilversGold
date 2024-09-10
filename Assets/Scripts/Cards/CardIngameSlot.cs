using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CardIngameSlot : MonoBehaviour, IDropHandler
{

    public string position;
    
    [HideInInspector]public CardManager cardManager;
    [HideInInspector]public DragDrop dragDrop;
    [HideInInspector]public BattleSystem battleSystem;

    private void Start()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && battleSystem.state == BattleState.PLAYERTURN)
        {
            cardManager = eventData.pointerDrag.GetComponentInParent<CardManager>();
            dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();
            
            if (cardManager.GetComponent<CardDisplay>().card.position == position)
            {
                cardManager.foundSlot = true;
                dragDrop.foundSlot = true;
                cardManager.CardPlayed();
                eventData.pointerDrag.GetComponent<DragDrop>().rectTransform.position =
                    GetComponent<RectTransform>().position;
            }
        }
    }
}
