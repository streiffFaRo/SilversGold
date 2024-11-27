using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardIngameSlot : MonoBehaviour, IDropHandler 
{
    //Verantwortlich für das Kontrollieren und Platzieren der Karten

    [Header("General")]
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

    public void OnDrop(PointerEventData eventData) //Wenn Karte auf Slot gezogen: Setzt Karte auf ihren Slot
    {
        if (eventData.pointerDrag != null && battleSystem.state == BattleState.PLAYERTURN)
        {
            currentCard = eventData.pointerDrag.GetComponentInParent<CardManager>();
            dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();

            if (currentCard.GetComponent<CardDisplay>().card.position == slotPosition)
            {
                if (currentCard.cardCommandPowerCost <= playerManager.currentCommandPower)
                {
                    currentCard.cardIngameSlot = this; //Auf der Karte wird der gefundene Slot gesetzt
                    dragDrop.foundSlot = true; //DragDrop Komponente wird informiert dass ein Slot gefunden wurde
                    currentCard.CardPlayed();
                    eventData.pointerDrag.GetComponent<DragDrop>().rectTransform.position =
                        GetComponent<RectTransform>().position;
                }
                else
                {
                    currentCard.animator.SetTrigger("trigger_cost_warn");
                    playerManager.commandPowerAnimator.SetTrigger("trigger_commandpower_warn");
                    VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
                }
            }
            else
            {
                currentCard.animator.SetTrigger("trigger_position_warn");
                VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
            }
        }
    }

    public void EnemyCardPlacedOnThisSlot(CardManager cardToPlaceOnSlot) //Setzt gegnerische Karte auf ihren Slot
    {
        currentCard = cardToPlaceOnSlot;
        currentCard.cardIngameSlot = this;
        currentCard.currentCardMode = CardMode.INPLAY;
        currentCard.GetComponentInChildren<DragDrop>().rectTransform.position = GetComponent<RectTransform>().position;
        currentCard.CardPlayed();
        currentCard.GetComponentInChildren<DragDrop>().GameObject().SetActive(false);
        
    }
}
