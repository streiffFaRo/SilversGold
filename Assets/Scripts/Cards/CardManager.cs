using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CardManager : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public bool hasBeenPlayed;
    public int handIndex;

    public GameObject handCard;
    public GameObject inGameCard;

    private DeckManager deckManager;
    private int cardCommandPowerCost;
    private BattleSystem battleSystem;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startDragPos;

    [HideInInspector] public bool foundSlot = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        cardCommandPowerCost = GetComponent<CardDisplay>().card.cost;
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    #region Drag&Drop

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        startDragPos = GetComponent<RectTransform>().position;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (!foundSlot && hasBeenPlayed == false)
        {
            GetComponent<RectTransform>().position = startDragPos;
        }
    }
    #endregion
    
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (hasBeenPlayed == false && battleSystem.state == BattleState.PLAYERTURN && false)
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
                Debug.LogWarning("Zu wenig Command Power!");
            }
        }
    }

    public void CardPlayed()
    {
        handCard.SetActive(false);
        inGameCard.SetActive(true);
        hasBeenPlayed = true;
        deckManager.availableCardSlots[handIndex] = true;
        deckManager.UpdateCommandPower(cardCommandPowerCost);
        
    }

    public void Attack()
    {
        
    }

    public void Retreat()
    {
        
    }

    public void Death()
    {
        
    }

    void MoveToDiscardPile()
    {
        deckManager.discardPile.Add(this);
        gameObject.SetActive(false);
    }
    
}
