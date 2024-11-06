using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    
    public Canvas canvas;
    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Vector2 startDragPos;

    [HideInInspector] public bool foundSlot = false;
    
    private void Awake()
    {
        canvasGroup = GetComponentInParent<CanvasGroup>();
    }

    private void Start()
    {
        foreach (Canvas can in FindObjectsOfType<Canvas>())
        {
            if (can.CompareTag("Content"))
            {
                canvas = can;
            }
        }
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
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (!foundSlot)
        {
            rectTransform.position = startDragPos;
            rectTransform.position -= new Vector3(0, 175*canvas.scaleFactor); //Negate Card Hover Position
            VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
}
