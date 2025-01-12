using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInfoBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Verantwortlich gewünschten Tooltip anzuzeigen

    [Header("General")]
    public string textToDisplay;
    public GameObject box;
    public Canvas canvas;
    
    //Private Variablen
    private TextMeshProUGUI text;
    private bool hovering;
    private Vector3 mousePosition;
    //private GameObject boxRef;
    private Vector2 tooltipOffset = new Vector2(100, -50);

    private void Start()
    {
       mousePosition = Input.mousePosition;
    }

    private void Update()
    {
        if (hovering)
        {
            UpdateTooltipPosition();
        }
    }

    private void UpdateTooltipPosition()
    {
        Vector2 localPoint;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition, null,
            out localPoint);
        
        localPoint += tooltipOffset;
        
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Prüfen, ob sich die Maus am rechten Rand befindet und Tooltip nach links verschieben
        if (Input.mousePosition.x > screenWidth * 0.75f)
        {
            localPoint.x -= tooltipOffset.x * 2;  // Tooltip nach links verschieben
        }

        // Prüfen, ob sich die Maus am unteren Rand befindet und Tooltip nach oben verschieben
        if (Input.mousePosition.y < screenHeight * 0.25f)
        {
            localPoint.y -= tooltipOffset.y * 2f;  // Tooltip nach oben verschieben
        }
        
        box.GetComponent<RectTransform>().localPosition = localPoint;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //boxRef = Instantiate(box, canvas.transform, canvas.transform);
        //box.transform.SetSiblingIndex(transform.parent.childCount-1);
        hovering = true;
        box.SetActive(true);
        text = box.GetComponentInChildren<TextMeshProUGUI>();
        text.text = textToDisplay;
        
        UpdateTooltipPosition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (box.activeSelf)
        {
            //Destroy(boxRef);
            box.SetActive(false);
            hovering = false;
        }
    }
}
