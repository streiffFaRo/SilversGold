using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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
    
    [Header("Other")]
    public GameObject damageCounterFolder;
    public GameObject damageCounterPrefab;
    [HideInInspector]public int cardCommandPowerCost;

    [Header("CardInPlayInformation")]
    public bool cardActed;
    public CardIngameSlot cardIngameSlot;
    public int currentHealth;

    [Header("Buttons")]
    public Button attackButton;
    public Button retreatButton;

    [Header("Animation")]
    public Animator animator;
    public GameObject cannonBallVisualRoot;

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
    private CardManager cardAttacked = null;  //Um zu bestimmen welche Karte angegriffen wird
    private CardManager cannoneerAttacked = null;
    private int broadsideDamage;
    private Tween moveTween;
    

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

        damageCounterFolder = FindObjectOfType<DamageCounterFolder>().gameObject;
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
    }

    public void CardPlayed()
    {
        cardActed = true;
        
        if (owner == Owner.PLAYER)
        {
            deckManager.availableCardSlots[handIndex] = true;
            playerManager.UpdateCommandPower(cardCommandPowerCost);
            deckManager.cardsInHand.Remove(this);
            DidCardAct();
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
        currentCardMode = CardMode.INPLAY;
        GetComponent<DiesWhenAlone>()?.CheckIfAlone();
        VolumeManager.instance.GetComponent<AudioManager>().PlayCardPlaySound();
    }

    #region CardInteraction
    
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
    
    #endregion
    
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

    #region Attack
    
    public void Attack()
    {
        
        if (battleSystem.state == BattleState.PLAYERTURN && playerManager.currentCommandPower > 0 && owner == Owner.PLAYER)
        {
            if (cardStats.attack >= 1)
            {
                HandleAttack();
                
                if (cardStats.position == "I")
                {
                    if (cardIngameSlot.enemyInfantryLine.currentCard != null)
                    {
                        cardAttacked = cardIngameSlot.enemyInfantryLine.currentCard;
                    }
                    else if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
                    {
                        cardAttacked = cardIngameSlot.enemyArtilleryLine.currentCard;
                    }
                }
                else if (cardStats.position == "A")
                {
                    if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
                    {
                        cardAttacked = cardIngameSlot.enemyArtilleryLine.currentCard;
                    }
                }
                
                animator.SetTrigger("trigger_player_attack");

                if (cardAttacked != null) //If a Card is attacked -> Play Defense Animation
                {
                    cardAttacked.animator.SetTrigger("trigger_defense");
                }
                
            }
            else
            {
                Debug.LogWarning("Karte hat 0 Attack");
                animator.SetTrigger("trigger_attack_warn");
                VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
            }
        }
        
        else if (battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
        {
            //Wenn obere if nicht erfüllt, hat Spieler zwingen zu wenig CP
            Debug.LogWarning("Zu wenig CommandPower");
            playerManager.commandPowerAnimator.SetTrigger("trigger_commandpower_warn");
            VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
        }
        
        //ENEMY ATTACK
        else if (battleSystem.state == BattleState.ENEMYTURN && owner == Owner.ENEMY)
        {
            HandleAttack();
            
            if (cardStats.position == "I")
            {
                if (cardIngameSlot.enemyInfantryLine.currentCard != null)
                {
                    cardAttacked = cardIngameSlot.enemyInfantryLine.currentCard;
                }
                else if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
                {
                    cardAttacked = cardIngameSlot.enemyArtilleryLine.currentCard;
                }
            }
            else if (cardStats.position == "A")
            {
                if (cardIngameSlot.enemyArtilleryLine.currentCard != null)
                {
                    cardAttacked = cardIngameSlot.enemyArtilleryLine.currentCard;
                }
            }
            animator.SetTrigger("trigger_enemy_attack");
            
            if (cardAttacked != null) //If a Card is attacked -> Play Defense Animation
            {
                cardAttacked.animator.SetTrigger("trigger_defense");
            }
            
        }
        else
        {
            Debug.LogError("Attack failed!");
            VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
        }
    }

    public void HandleAttack()
    {
        cardActed = true;
        if (owner == Owner.PLAYER)
        {
            playerManager.UpdateCommandPower(1);
            SetButtonsPassive();
        }
        else if (owner == Owner.ENEMY)
        {
            enemyManager.UpdateEnemyCommandPower(1);
        }
    }

    public void DealAttackDamage()
    {
        //Damage dealt
        if (cardAttacked != null)
        {
            UpdateCardHealth(cardAttacked.cardStats.attack);
            cardAttacked.UpdateCardHealth(cardStats.attack);
            
            SpawnDamageCounter(rectTransform.position + new Vector3(75, 75,0), cardAttacked.cardStats.attack);
            SpawnDamageCounter(cardAttacked.rectTransform.position + new Vector3(75, 75, 0), cardStats.attack);
        }
        else
        {
            if (owner == Owner.PLAYER)
            {
                enemyManager.UpdateEnemyHealth(cardStats.attack, false);
                
                SpawnDamageCounter(enemyManager.enemyHealthText.rectTransform.position + new Vector3(75,-75,0), cardStats.attack);
            }
            else if (owner == Owner.ENEMY)
            {
                playerManager.UpdateHealth(cardStats.attack, false);
                
                SpawnDamageCounter(playerManager.healthText.rectTransform.position+ new Vector3(75,75,0), cardStats.attack);
            }
        }
        cardAttacked = null; //reset
        
        VolumeManager.instance.GetComponent<AudioManager>().PlayCardAttackSound();
        if (GetComponent<DiesAfterAttack>() != null)
        {
            Death();
        }
    }
    
    #endregion

    public void SpawnDamageCounter(Vector3 position, int amount)
    {
        GameObject createdDamageCounter = Instantiate(damageCounterPrefab, position, Quaternion.identity,
            damageCounterFolder.transform);
        createdDamageCounter.GetComponent<DamageCounter>().numberText.text = "-"+amount.ToString();
    }

    #region Retreat
    
    public void Retreat()
    {
        if (battleSystem.state == BattleState.PLAYERTURN && playerManager.currentCommandPower > 0 && owner == Owner.PLAYER)
        {
            playerManager.UpdateCommandPower(1);
            SetButtonsPassive();
            cardActed = true;
            hasActedRim.SetActive(false);
            VolumeManager.instance.GetComponent<AudioManager>().PlayCardRetreatSound();
            animator.SetTrigger("trigger_retreat");
            
        }
        else if (battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
        {
            //Wenn obere if nicht erfüllt, hat Spieler zwingen zu wenig CP
            Debug.LogWarning("Zu wenig CommandPower");
            playerManager.commandPowerAnimator.SetTrigger("trigger_commandpower_warn");
            VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
        }
        else if (battleSystem.state == BattleState.ENEMYTURN && owner == Owner.ENEMY)
        {
            enemyManager.UpdateEnemyCommandPower(1);
            cardActed = true;
            VolumeManager.instance.GetComponent<AudioManager>().PlayCardRetreatSound();
            animator.SetTrigger("trigger_retreat");
        }
        else
        {
            Debug.LogError("Retreat failed!");
        }
    }

    public void StartHandleRetreatStats()
    {
        StartCoroutine(HandleRetreatStats());
    }

    public IEnumerator HandleRetreatStats()
    {
        if (owner == Owner.PLAYER)
        {
            deckManager.deck.Add(this);
        }
        else if (owner == Owner.ENEMY)
        {
            enemyManager.deck.Add(this);
            enemyManager.UpdateEnemyUI();
            cardBG.SetActive(true);
        }
        currentCardMode = CardMode.INDECK;
        GetComponent<RetreatEffects>()?.TriggerRetreatEffect();
        //Hier war mal Verzögerung drin, bei problemen wieder einfügen
        currentHealth = cardStats.defense;
        deckManager.HideDisplayCard();
        cardIngameSlot.currentCard = null;
        foreach (DiesWhenAlone var in FindObjectsOfType<DiesWhenAlone>())
        {
            if (var.GetComponent<CardManager>().currentCardMode == CardMode.INPLAY)
            {
                var.CheckIfAlone();
            }
        }
        handCard.SetActive(true);
        handCard.transform.localScale = new Vector3(1f, 1f, 1f);
        GetComponentInChildren<DragDrop>(true).gameObject.SetActive(true);
        GetComponentInChildren<DragDrop>().foundSlot = false;
        inGameCard.SetActive(false);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
    }

    #endregion

    #region Broadside
    
    public void Broadside()
    {
        cannonBallVisualRoot.SetActive(true);
        
        if (battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
        {
            SetButtonsPassive();
            cardActed = true;
            DidCardAct();
            
            if (cardIngameSlot.enemyArtilleryLine.currentCard != null) //Spieler Kanonier greift gegnerische Arty Karte an
            { 
                CannonBallAnimation(900f,0.8f, BroadsideEffects);
                cannoneerAttacked = cardIngameSlot.enemyArtilleryLine.currentCard;
                broadsideDamage = GameManager.instance.shipCannonLevel + 1;

            }
            else //Spieler Kanonier greift gegnerisches Schiff an
            { 
                CannonBallAnimation(1100f,0.8f, BroadsideEffects);
            }
            
        }
        else if (battleSystem.state == BattleState.ENEMYTURN && owner == Owner.ENEMY)
        {
            cardActed = true;
            
            if (cardIngameSlot.enemyArtilleryLine.currentCard != null) //Gegner Kanonier greift spieler Artya Karte an
            { 
                CannonBallAnimation(-900f,0.8f, BroadsideEffects);
                cannoneerAttacked = cardIngameSlot.enemyArtilleryLine.currentCard;
                broadsideDamage = enemyManager.enemyCannonLevel;
            }
            else //Gegner greift spieler Schiff an
            { 
                CannonBallAnimation(-1100f,0.8f, BroadsideEffects);
            }
        }
        else
        {
            Debug.LogWarning("Attack failed!");
        }
    }

    private void BroadsideEffects()
    {
        cannonBallVisualRoot.SetActive(false);

        if (cannoneerAttacked != null)
        {
            SpawnDamageCounter(cannoneerAttacked.rectTransform.position + new Vector3(75,-75,0), broadsideDamage);
            cannoneerAttacked.UpdateCardHealth(broadsideDamage);
        }
        else
        {
            if (owner == Owner.PLAYER)
            {
                SpawnDamageCounter(enemyManager.enemyHealthText.rectTransform.position + new Vector3(75,-75,0), GameManager.instance.shipCannonLevel+1);
                enemyManager.UpdateEnemyHealth(GameManager.instance.shipCannonLevel+1, false);
            }
            else if (owner == Owner.ENEMY)
            {
                playerManager.UpdateHealth(enemyManager.enemyCannonLevel, false);
                SpawnDamageCounter(playerManager.healthText.rectTransform.position+ new Vector3(75,75,0), cardStats.attack);
            }
        }
    }

    #endregion

    public void DidCardAct()
    {
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
        if (cardIngameSlot != null)
        {
            if (cardIngameSlot.currentCard != null)
            {
                cardIngameSlot.currentCard = null;
            }
        }
        
        if (owner == Owner.PLAYER)
        {
            deckManager.discardPile.Add(this);
            GetComponentInChildren<DragDrop>(true).gameObject.SetActive(true);
            GetComponentInChildren<DragDrop>().foundSlot = false;
            hasActedRim.SetActive(false);
        }
        else if (owner == Owner.ENEMY)
        {
            enemyManager.discardPile.Add(this);
            enemyManager.UpdateEnemyUI();
            cardBG.SetActive(true);
        }
        GetComponent<DeathEffects>()?.TriggerDeathEffect();
        VolumeManager.instance.GetComponent<AudioManager>().PlayCardDeathSound();
        
        foreach (DiesWhenAlone var in FindObjectsOfType<DiesWhenAlone>())
        {
            if (var.GetComponent<CardManager>().currentCardMode == CardMode.INPLAY)
            {
                var.CheckIfAlone();
            }
        }
        
        currentCardMode = CardMode.INDECK;
        currentHealth = cardStats.defense;
        cardDisplay.SetUpCardUI();
        handCard.SetActive(true);
        inGameCard.SetActive(false);
        gameObject.SetActive(false);
    }

    #region Animation

    public void SetActedRimActive()
    {
        hasActedRim.SetActive(true);
    }

    public void SetActedRimPassive()
    {
        hasActedRim.SetActive(false);
    }
    
    private void CannonBallAnimation(float distance, float duration, TweenCallback onEnd)
    {

        if (moveTween!= null)
        {
            moveTween.Kill(false);
        }

        cannonBallVisualRoot.SetActive(true);
        cannonBallVisualRoot.transform.position = transform.position;
        moveTween = cannonBallVisualRoot.transform.DOMoveY(distance, duration);
        moveTween.onComplete += onEnd;
    }

    #endregion
    
}

public enum Owner
{
    PLAYER, ENEMY
}

public enum CardMode
{
    INHAND, INPLAY, INRECRUIT, INDECK, TODISCARD
}
