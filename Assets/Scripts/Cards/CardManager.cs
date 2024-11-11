using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Button = UnityEngine.UI.Button;


public class CardManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Verantwortlich für Karteninformation in Hand und im Kampf

    [Header("General")]
    public Owner owner;

    [Header("CardModes")] 
    public CardMode currentCardMode = CardMode.INDECK;
    public GameObject handCard;
    public GameObject inGameCard;
    public GameObject cardBG;
    public GameObject hasActedRim;
    
    public int handIndex;
    [HideInInspector]public int cardCommandPowerCost;

    [Header("CardInPlayInformation")]
    public bool cardActed;
    public CardIngameSlot cardIngameSlot;
    public int currentHealth;

    [Header("Buttons")]
    public Button attackButton;
    public Button retreatButton;

    //Private Scripts
    private DeckManager deckManager;
    private BattleSystem battleSystem;
    private PlayerManager playerManager;
    private EnemyManager enemyManager;
    private CardDisplay cardDisplay;
    private RecruitManager recruitManager;
    private PresentDeck presentDeck;
    [HideInInspector]public Card cardStats;
    
    //Private Variablen
    private float hoverTimer = 0;
    private bool isHovering;
    private RectTransform rectTransform;
    

    private void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        battleSystem = FindObjectOfType<BattleSystem>();
        playerManager = FindObjectOfType<PlayerManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
        cardStats = GetComponent<CardDisplay>().card;
        cardDisplay = GetComponent<CardDisplay>();
        recruitManager = FindObjectOfType<RecruitManager>();
        presentDeck = FindObjectOfType<PresentDeck>();
        rectTransform = GetComponent<RectTransform>();
        
        cardCommandPowerCost = cardStats.cost;
        currentHealth = cardStats.defense;
        hoverTimer = 0;
    }

    private void Update()
    {
        //Zählt wie lange der Spieler auf der Karte hovered
        if (isHovering)
        {
            hoverTimer += Time.deltaTime;
            
            if (hoverTimer >= 0.5f)
            {
                deckManager.ShowDisplayCard(this);
            }
        }

        //Zeigt ob die Karte schon gehandelt hat
        if (!cardActed && currentCardMode == CardMode.INPLAY && battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
        {
            hasActedRim.SetActive(true);
        }
        else
        {
            hasActedRim.SetActive(false);
        }
    }

    public void CardPlayed()
    {
        if (owner == Owner.PLAYER)
        {
            deckManager.availableCardSlots[handIndex] = true;
            playerManager.UpdateCommandPower(cardCommandPowerCost);
            deckManager.cardsInHand.Remove(this);
        }
        else if (owner == Owner.ENEMY)
        {
            cardBG.SetActive(false);
            enemyManager.availableHandCardSlots[handIndex] = true;
            enemyManager.UpdateEnemyCommandPower(cardCommandPowerCost);
            enemyManager.cardsInHand.Remove(this);
        }
        handCard.SetActive(false);
        inGameCard.SetActive(true);
        cardActed = true;
        currentCardMode = CardMode.INPLAY;
        GetComponent<DiesWhenAlone>()?.CheckIfAlone();
        VolumeManager.instance.GetComponent<AudioManager>().PlayCardPlaySound();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (battleSystem != null)
        {
            if (currentCardMode == CardMode.INPLAY && battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
            {
                if (!attackButton.gameObject.activeInHierarchy)
                {
                    deckManager.SetAllOtherButtonsPassive(this);
                }
                else
                {
                    SetButtonsPassive();
                }
            }
            else if (currentCardMode == CardMode.INRECRUIT)
            {
                recruitManager.CardChoosen(cardStats);
            }
            else if (currentCardMode == CardMode.TODISCARD)
            {
                presentDeck.DiscardCard(cardStats);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (battleSystem != null)
        {
            if (currentCardMode == CardMode.INPLAY && battleSystem.state != BattleState.WON && battleSystem.state != BattleState.LOST)
            {
                isHovering = true;
            }
            else if (currentCardMode == CardMode.INHAND && battleSystem.state != BattleState.WON && battleSystem.state != BattleState.LOST)
            {
                VolumeManager.instance.GetComponent<AudioManager>().PlayCardHandHoverSound();
                transform.SetSiblingIndex(transform.parent.childCount-1);
                transform.localScale = new Vector3(2f, 2f, 2f);
                cardDisplay.ShowKeyWordBox();
                rectTransform.localPosition += new Vector3(0, 175);
            }
            else if (currentCardMode == CardMode.INRECRUIT)
            {
                VolumeManager.instance.GetComponent<AudioManager>().PlayCardHandHoverSound();
            }
            else if (currentCardMode == CardMode.INDECK && battleSystem.state != BattleState.PLAYERTURN && battleSystem.state != BattleState.ENEMYTURN)
            {
                transform.SetSiblingIndex(transform.parent.childCount-1);
                transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                cardDisplay.ShowKeyWordBox();
                VolumeManager.instance.GetComponent<AudioManager>().PlayCardHandHoverSound();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (battleSystem != null)
        {
            if (currentCardMode == CardMode.INPLAY)
            {
                deckManager.HideDisplayCard();
            }
            else if (currentCardMode == CardMode.INHAND)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                cardDisplay.HideKeyWordBox();
                rectTransform.localPosition += new Vector3(0, -175);
            }
            else if (currentCardMode == CardMode.INDECK && battleSystem.state != BattleState.PLAYERTURN && battleSystem.state != BattleState.ENEMYTURN)
            {
                transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                cardDisplay.HideKeyWordBox();
            }
            hoverTimer = 0;
            isHovering = false;
        }
        
    }
    
    public void SetButtonsActive()
    {
        attackButton.gameObject.SetActive(true);
        retreatButton.gameObject.SetActive(true);
    }

    public void SetButtonsPassive()
    {
        attackButton.gameObject.SetActive(false);
        retreatButton.gameObject.SetActive(false);
    }

    public void Attack()
    {
        //TODO: Effects
        if (battleSystem.state == BattleState.PLAYERTURN && playerManager.currentCommandPower > 0 && owner == Owner.PLAYER)
        {
            if (cardStats.attack >= 1)
            {
                if (cardStats.position == "I")
                {
                    if (cardIngameSlot.enemyInfantryLine.currentCard != null)
                    {
                        UpdateCardHealth(cardIngameSlot.enemyInfantryLine.currentCard.cardStats.attack);
                        cardIngameSlot.enemyInfantryLine.currentCard.UpdateCardHealth(cardStats.attack);
                    }
                    else if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
                    {
                        UpdateCardHealth(cardIngameSlot.enemyArtilleryLine.currentCard.cardStats.attack);
                        cardIngameSlot.enemyArtilleryLine.currentCard.UpdateCardHealth(cardStats.attack);
                    }
                    else
                    {
                        enemyManager.UpdateEnemyHealth(cardStats.attack, false);
                    }
                }
                else if (cardStats.position == "A")
                {
                    if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
                    {
                        UpdateCardHealth(cardIngameSlot.enemyArtilleryLine.currentCard.cardStats.attack);
                        cardIngameSlot.enemyArtilleryLine.currentCard.UpdateCardHealth(cardStats.attack);
                    }
                    else
                    {
                        enemyManager.UpdateEnemyHealth(cardStats.attack, false);
                    }
                }
                HandleAttackStats();
            }
            else
            {
                Debug.LogWarning("Karte hat 0 Attack");
                VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
                //TODO Info an Player -> Attack Animation
            }
        }
        else if (battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
        {
            //Wenn obere if nicht erfüllt, hat Spieler zwingen zu wenig CP
            Debug.LogWarning("Zu wenig CommandPower");
            VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
            //TODO Info an Player -> CommandPower Animation
        }
        //ENEMY ATTACK
        else if (battleSystem.state == BattleState.ENEMYTURN && owner == Owner.ENEMY)
        {
            if (cardStats.position == "I")
            {
                if (cardIngameSlot.enemyInfantryLine.currentCard != null)
                {
                    UpdateCardHealth(cardIngameSlot.enemyInfantryLine.currentCard.cardStats.attack);
                    cardIngameSlot.enemyInfantryLine.currentCard.UpdateCardHealth(cardStats.attack);
                }
                else if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
                {
                    UpdateCardHealth(cardIngameSlot.enemyArtilleryLine.currentCard.cardStats.attack);
                    cardIngameSlot.enemyArtilleryLine.currentCard.UpdateCardHealth(cardStats.attack);
                }
                else
                {
                    playerManager.UpdateHealth(cardStats.attack, false);
                }
            }
            else if (cardStats.position == "A")
            {
                if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
                {
                    UpdateCardHealth(cardIngameSlot.enemyArtilleryLine.currentCard.cardStats.attack);
                    cardIngameSlot.enemyArtilleryLine.currentCard.UpdateCardHealth(cardStats.attack);
                }
                else
                {
                    playerManager.UpdateHealth(cardStats.attack, false);
                }
            }
            HandleAttackStats();
        }
        else
        {
            Debug.LogError("Attack failed!");
            VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
        }
    }

    public void HandleAttackStats()
    {
        if (owner == Owner.PLAYER)
        {
            playerManager.UpdateCommandPower(1);
            SetButtonsPassive();
        }
        else if (owner == Owner.ENEMY)
        {
            enemyManager.UpdateEnemyCommandPower(1);
        }
        VolumeManager.instance.GetComponent<AudioManager>().PlayCardAttackSound();
        cardActed = true;
        if (GetComponent<DiesAfterAttack>() != null)
        {
            Death();
        }
        //TODO Karte soll symbolisieren dass sie genutzt wurde
    }

    public void Retreat()
    {
        if (battleSystem.state == BattleState.PLAYERTURN && playerManager.currentCommandPower > 0 && owner == Owner.PLAYER)
        {
            playerManager.UpdateCommandPower(1);
            SetButtonsPassive();
            deckManager.deck.Add(this);
            StartCoroutine(HandleRetreatStats());
        }
        else if (battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
        {
            //Wenn obere if nicht erfüllt, hat Spieler zwingen zu wenig CP
            Debug.LogWarning("Zu wenig CommandPower");
            VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
            //TODO Info an Player
        }
        else if (battleSystem.state == BattleState.ENEMYTURN && owner == Owner.ENEMY)
        {
            enemyManager.UpdateEnemyCommandPower(1);
            enemyManager.deck.Add(this);
            enemyManager.UpdateEnemyUI();
            cardBG.SetActive(true);
            StartCoroutine(HandleRetreatStats());
        }
        else
        {
            Debug.LogError("Retreat failed!");
        }
    }
    
    private IEnumerator HandleRetreatStats()
    {
        currentCardMode = CardMode.INDECK;
        GetComponent<RetreatEffects>()?.TriggerRetreatEffect();
        yield return new WaitForSeconds(0.05f);
        currentHealth = cardStats.defense;
        deckManager.HideDisplayCard();
        cardIngameSlot.currentCard = null;
        foreach (DiesWhenAlone var in FindObjectsOfType<DiesWhenAlone>())
        {
            var.CheckIfAlone();
        }
        handCard.SetActive(true);
        handCard.transform.localScale = new Vector3(1f, 1f, 1f);
        GetComponentInChildren<DragDrop>(true).gameObject.SetActive(true);
        GetComponentInChildren<DragDrop>().foundSlot = false;
        VolumeManager.instance.GetComponent<AudioManager>().PlayCardRetreatSound();
        inGameCard.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Broadside()
    {
        if (battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
        {
            if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
            { 
                cardIngameSlot.enemyArtilleryLine.currentCard.UpdateCardHealth(GameManager.instance.shipCannonLevel+1);
            }
            else
            { 
                enemyManager.UpdateEnemyHealth(GameManager.instance.shipCannonLevel+1, false);
            }
            SetButtonsPassive();
            cardActed = true;
            //TODO Animation
        }
        else if (battleSystem.state == BattleState.ENEMYTURN && owner == Owner.ENEMY)
        {
            if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
            { 
                cardIngameSlot.enemyArtilleryLine.currentCard.UpdateCardHealth(enemyManager.enemyCannonLevel);
            }
            else
            { 
                playerManager.UpdateHealth(enemyManager.enemyCannonLevel, false);
            }
            cardActed = true;
        }
        else
        {
            Debug.LogWarning("Attack failed!");
        }
    }

    public void UpdateCardHealth(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Death();
        }
        else if (currentHealth >= cardStats.defense)
        {
            cardDisplay.inGameDefenseText.text = cardStats.defense.ToString();
            cardDisplay.inGameDefenseText.color = Color.white;
        }
        else
        {
            cardDisplay.inGameDefenseText.text = currentHealth.ToString();
            cardDisplay.inGameDefenseText.color = Color.red;
        }
    }
    
    public void Death()
    {
        if (owner == Owner.PLAYER)
        {
            deckManager.discardPile.Add(this);
            GetComponentInChildren<DragDrop>(true).gameObject.SetActive(true);
            GetComponentInChildren<DragDrop>().foundSlot = false;
        }
        else if (owner == Owner.ENEMY)
        {
            enemyManager.discardPile.Add(this);
            enemyManager.UpdateEnemyUI();
            cardBG.SetActive(true);
        }
        GetComponent<DeathEffects>()?.TriggerDeathEffect();
        VolumeManager.instance.GetComponent<AudioManager>().PlayCardDeathSound();
        cardIngameSlot.currentCard = null;
        foreach (DiesWhenAlone var in FindObjectsOfType<DiesWhenAlone>())
        {
            var.CheckIfAlone();
        }
        currentCardMode = CardMode.INDECK;
        currentHealth = cardStats.defense;
        cardDisplay.SetUpCardUI();
        handCard.SetActive(true);
        inGameCard.SetActive(false);
        gameObject.SetActive(false);
    }
    
}

public enum Owner
{
    PLAYER, ENEMY
}

public enum CardMode
{
    INHAND, INPLAY, INRECRUIT, INDECK, TODISCARD
}
