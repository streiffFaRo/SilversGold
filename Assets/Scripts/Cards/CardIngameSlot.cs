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
            CardManager cardToCheck = eventData.pointerDrag.GetComponentInParent<CardManager>();

            if (cardToCheck.GetComponent<CardDisplay>().card.position == slotPosition)
            {
                if (cardToCheck.cardCommandPowerCost <= playerManager.currentCommandPower)
                {
                    currentCard = cardToCheck;
                    dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();
                    currentCard.cardIngameSlot = this; //Auf der Karte wird der gefundene Slot gesetzt
                    dragDrop.foundSlot = true; //DragDrop Komponente wird informiert dass ein Slot gefunden wurde
                    currentCard.CardPlayed();
                    eventData.pointerDrag.GetComponent<DragDrop>().rectTransform.position =
                        GetComponent<RectTransform>().position;
                }
                else
                {
                    cardToCheck.animator.SetTrigger("trigger_cost_warn");
                    playerManager.commandPowerAnimator.SetTrigger("trigger_commandpower_warn");
                    VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
                }
            }
            else
            {
                cardToCheck.animator.SetTrigger("trigger_position_warn");
                VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
            }
        }
    }

    public void EnemyCardPlacedOnThisSlot(CardManager cardToPlaceOnSlot) //Setzt gegnerische Karte auf ihren Slot
    {
        currentCard = cardToPlaceOnSlot;
        currentCard.cardIngameSlot = this;
        currentCard.currentCardMode = CardMode.INPLAY;
        currentCard.GetComponentInChildren<DragDrop>(true).gameObject.SetActive(true);
        currentCard.GetComponentInChildren<DragDrop>().rectTransform.position = GetComponent<RectTransform>().position;
        currentCard.CardPlayed();
        currentCard.GetComponentInChildren<DragDrop>().GameObject().SetActive(false);
        
    }
}
