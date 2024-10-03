using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    
    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startDragPos;

    [HideInInspector] public bool foundSlot = false;
    
    private void Awake()
    {
        canvasGroup = GetComponentInParent<CanvasGroup>();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        startDragPos = rectTransform.position;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (!foundSlot)
        {
            rectTransform.position = startDragPos;
            rectTransform.position -= new Vector3(0, 125); //Negate Card Hover Position
            
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
}
