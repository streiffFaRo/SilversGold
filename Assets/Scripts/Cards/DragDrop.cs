using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //Verantwortlich für Verschieben der Karte bis sie platziert ist
    
    [Header("General")]
    public Canvas canvas;
    public RectTransform rectTransform;
    public Vector2 startDragPos;
    [HideInInspector] public bool foundSlot = false;
    
    //Priavte Komponente
    private CanvasGroup canvasGroup;
    
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
        //Absichtlich leer
    }
    
    public void OnBeginDrag(PointerEventData eventData) 
    {
        //Karte wird durchsichtig
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        startDragPos = rectTransform.position;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; //Karte folgt Maus (wird gezogen)
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (!foundSlot)
        {
            rectTransform.position = startDragPos; //Setzt sich auf Handposition zurück
            rectTransform.position -= new Vector3(0, 175*canvas.scaleFactor); //Negate Card Hover Position
        }
        else
        {
            gameObject.SetActive(false); //Wenn slot gefunden, siehe CardIngameSlot.cs, schaltet sich dieses Script aus
        }
    }
    
}
