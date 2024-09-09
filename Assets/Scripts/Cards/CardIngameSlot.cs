using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardIngameSlot : MonoBehaviour, IDropHandler
{

    public CardManager CardManager;
    public string position;
    
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            CardManager = eventData.pointerDrag.GetComponent<CardManager>();
            if (CardManager.GetComponent<CardDisplay>().card.position == position)
            {
                CardManager.foundSlot = true;
                CardManager.CardPlayed();
                eventData.pointerDrag.GetComponent<RectTransform>().position =
                    GetComponent<RectTransform>().position;
            }
        }
    }
}
