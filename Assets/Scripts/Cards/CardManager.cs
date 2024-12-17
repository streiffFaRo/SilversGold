using DG.Tweening;
using Unity.Mathematics;
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

    [Header("Particles")] 
    public ParticleSystem particleBlood;
    public ParticleSystem particleSmoke;
    
    [Header("Other")]
    public DamageCounterFolder damageCounterFolder;
    [HideInInspector]public int cardCommandPowerCost;
    public CardDisplay cardDisplay;

    //Private Scripts
    private DeckManager deckManager;
    private BattleSystem battleSystem;
    private PlayerManager playerManager;
    private EnemyManager enemyManager;
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
        //Setzt alle Variablen
        deckManager = FindObjectOfType<DeckManager>();
        battleSystem = FindObjectOfType<BattleSystem>();
        playerManager = FindObjectOfType<PlayerManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
        cardStats = GetComponent<CardDisplay>().card;
        cardDisplay = GetComponent<CardDisplay>();
        recruitManager = FindObjectOfType<RecruitManager>();
        presentDeck = FindObjectOfType<PresentDeck>();
        rectTransform = GetComponent<RectTransform>();
        damageCounterFolder = FindObjectOfType<DamageCounterFolder>();
        
        //Setzt schnellzugriffe für häufig genutzte Variablen
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
    }

    public void CardPlayed() //Wenn Karte Slot gefundet hat, siehe CardIngameSlot.cs, wird sie gespielt
    {
        cardActed = true; //Karte kann im ersten Zug nichts machen
        
        if (owner == Owner.PLAYER)
        {
            deckManager.availableCardSlots[handIndex] = true; //Handposition der Karte wird frei
            playerManager.UpdateCommandPower(cardCommandPowerCost); //Command Power wird abgezogen
            deckManager.cardsInHand.Remove(this); //Entfernt Karte aus der Liste der Handkarten
            DidCardAct(); //Rand wird gesetzt
            
            //Tutorial wird gezündet
            if (!GameManager.instance.tutorialDone && GameManager.instance.currentLevel == 1)
            {
                FindObjectOfType<Combat_Tutorial>()?.Tutorial2();
            }
        }
        else if (owner == Owner.ENEMY)
        {
            cardBG.SetActive(false);//Kartenrücken wird ausgeschaltet
            enemyManager.availableHandCardSlots[handIndex] = true; //Handposition der Karte wird frei
            enemyManager.UpdateEnemyCommandPower(cardCommandPowerCost); //Command Power wird abgezogen
            enemyManager.cardsInHand.Remove(this); //Entfernt Karte aus der Liste der Handkarten
        }
        
        handCard.SetActive(false); //Visuell wird Handkarte passiv geschalten
        inGameCard.SetActive(true); //Visuell wird Spielkarte aktiv geschalten
        currentCardMode = CardMode.INPLAY; //Kartenmodus wird geändert
        GetComponent<DiesWhenAlone>()?.CheckIfAlone();
        VolumeManager.instance.GetComponent<AudioManager>().PlayCardPlaySound(); //Sound
    }

    #region CardInteraction
    
    public void OnPointerClick(PointerEventData eventData) //Effekt wenn man auf Karte klickt
    {
        if (battleSystem != null)
        {
            if (currentCardMode == CardMode.INPLAY && battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
            {
                if (!attackButton.gameObject.activeInHierarchy)
                {
                    deckManager.SetAllOtherButtonsPassive(this); //Zeigt Aktionensbuttons
                }
                else
                {
                    SetButtonsPassive();
                }
            }
            else if (currentCardMode == CardMode.INRECRUIT)
            {
                recruitManager.CardChoosen(cardStats); //Rekrutiert Karte ins Spielerdeck
            }
            else if (currentCardMode == CardMode.TODISCARD)
            {
                presentDeck.DiscardCard(cardStats); //Verwirft die Karte aus dem Deck um Platz zu machen
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) //Effekt wenn man auf Karte hovered
    {
        if (battleSystem != null)
        {
            if (currentCardMode == CardMode.INPLAY && battleSystem.state != BattleState.WON && battleSystem.state != BattleState.LOST)
            {
                isHovering = true; //Aktiviert Hovering-Timer
            }
            else if (currentCardMode == CardMode.INHAND && battleSystem.state != BattleState.WON && battleSystem.state != BattleState.LOST && owner == Owner.PLAYER)
            {
                //Vergrössert die Karte in der Hand
                VolumeManager.instance.GetComponent<AudioManager>().PlayCardHandHoverSound(); 
                transform.SetSiblingIndex(transform.parent.childCount-1);
                transform.localScale = new Vector3(2f, 2f, 2f);
                cardDisplay.ShowKeyWordBox();
                rectTransform.localPosition += new Vector3(0, 175);
            }
            else if (currentCardMode == CardMode.INRECRUIT)
            {
                //Sound wenn man über Karte hovered im Rekrutierungsmodus
                VolumeManager.instance.GetComponent<AudioManager>().PlayCardHandHoverSound(); 
            }
            else if (currentCardMode == CardMode.INDECK && battleSystem.state != BattleState.PLAYERTURN && battleSystem.state != BattleState.ENEMYTURN)
            {
                //Vergrössert die Karte im Deckanschaumodus
                transform.SetSiblingIndex(transform.parent.childCount-1); 
                transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                cardDisplay.ShowKeyWordBox();
                VolumeManager.instance.GetComponent<AudioManager>().PlayCardHandHoverSound();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) //Effekt wenn man nicht mehr auf der Karte hovered
    {
        if (battleSystem != null)
        {
            if (currentCardMode == CardMode.INPLAY)
            {
                deckManager.HideDisplayCard(); //Zeigt keine Kartenvorschau mehr
            }
            else if (currentCardMode == CardMode.INHAND)
            {
                //Verkleinert Karte in der Hand wieder
                transform.localScale = new Vector3(1f, 1f, 1f);
                cardDisplay.HideKeyWordBox();
                rectTransform.localPosition += new Vector3(0, -175);
            }
            else if (currentCardMode == CardMode.INDECK && battleSystem.state != BattleState.PLAYERTURN && battleSystem.state != BattleState.ENEMYTURN)
            {
                //Verkleinert Karte in der Deckanschau wieder
                transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                cardDisplay.HideKeyWordBox();
            }
            //Beendet den Hover-Timer
            hoverTimer = 0;
            isHovering = false;
        }
    }
    
    public void SetButtonsActive() //Zeigt Aktionsbuttons im Kampf (Angriff/Rückzug)
    {
        attackButton.gameObject.SetActive(true);
        retreatButton.gameObject.SetActive(true);
    }

    public void SetButtonsPassive() //Versteckt Aktionsbuttons
    {
        attackButton.gameObject.SetActive(false);
        retreatButton.gameObject.SetActive(false);
    }
    
    #endregion

    #region Attack
    
    public void Attack()
    {
        //Player Attack
        if (battleSystem.state == BattleState.PLAYERTURN && playerManager.currentCommandPower > 0 && owner == Owner.PLAYER)
        {
            if (cardStats.attack >= 1)
            {
                HandleAttack(); //Führt Aktionen vor Angriffsanimation aus
                
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
                
                animator.SetTrigger("trigger_player_attack"); //Startet Angriffsanimation

                if (cardAttacked != null) //Wenn eine Karte angegriffen wird -> Spielt die Verteidigungsanimation
                {
                    cardAttacked.animator.SetTrigger("trigger_defense");
                }
            }
            else
            {
                //Info an Spieler, dass Karten mit 0 Angriff nicht angreiffen können
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
            HandleAttack(); //Führt Aktionen vor Angriffsanimation aus
            
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
            animator.SetTrigger("trigger_enemy_attack"); //Startet Angriffsanimation
            
            if (cardAttacked != null) //Wenn eine Karte angegriffen wird -> Spielt die Verteidigungsanimation
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

    public void HandleAttack() //Entzieht Command Power & zeigt, dass Karte gehandelt hat
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

    public void DealAttackDamage() //Verrechnet Schaden
    {
        if (cardAttacked != null)
        {
            UpdateCardHealth(cardAttacked.cardStats.attack);
            cardAttacked.UpdateCardHealth(cardStats.attack);

            ParticleSystem particlesAttacker = Instantiate(particleBlood, rectTransform.position, quaternion.identity, damageCounterFolder.transform); //Blood Particles Attacker
            particlesAttacker.Play();
            damageCounterFolder.SpawnDamageCounter(rectTransform.position + new Vector3(75, 75,0), cardAttacked.cardStats.attack);
            
            ParticleSystem particlesDefender = Instantiate(particleBlood, cardAttacked.rectTransform.position, quaternion.identity, damageCounterFolder.transform); //Blood Particles Defender
            particlesDefender.Play();
            damageCounterFolder.SpawnDamageCounter(cardAttacked.rectTransform.position + new Vector3(75, 75, 0), cardStats.attack);
        }
        else
        {
            if (owner == Owner.PLAYER)
            {
                enemyManager.UpdateEnemyHealth(cardStats.attack, false);
                
                damageCounterFolder.SpawnDamageCounter(enemyManager.enemyHealthText.rectTransform.position + new Vector3(75,-75,0), cardStats.attack);
            }
            else if (owner == Owner.ENEMY)
            {
                playerManager.UpdateHealth(cardStats.attack, false);
                
                damageCounterFolder.SpawnDamageCounter(playerManager.healthText.rectTransform.position+ new Vector3(75,75,0), cardStats.attack);
            }
        }
        cardAttacked = null; //reset
        
        VolumeManager.instance.GetComponent<AudioManager>().PlayCardAttackSound();
        
        if (GetComponent<DiesAfterAttack>() != null)
        {
            Death(); //Töten Karten, die nach angreiffen sterben
        }
    }
    
    #endregion

    #region Retreat
    
    public void Retreat()
    {
        if (battleSystem.state == BattleState.PLAYERTURN && playerManager.currentCommandPower > 0 && owner == Owner.PLAYER)
        {
            //Spieler Karte zieht sich zurück
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
            //Gegner Karte zieht sich zurück
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

    public void HandleRetreatStats()
    {
        //Setzt Karte zurück ins Deck
        if (owner == Owner.PLAYER)
        {
            deckManager.deck.Add(this);
        }
        else if (owner == Owner.ENEMY)
        {
            enemyManager.deck.Add(this);
        }
        GetComponent<RetreatEffects>()?.TriggerRetreatEffect(); //Löst Effekt aus
        
        ResetCard();
    }

    #endregion

    #region Broadside
    
    public void Broadside()
    {
        cannonBallVisualRoot.SetActive(true); //Kanonenkugel wird sichtbar geschalten
        VolumeManager.instance.GetComponent<AudioManager>().PlayCannonSound(); //Sound
        
        if (battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER) //Spieler Breitseite
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
                cannoneerAttacked = null;
            }
            
        }
        else if (battleSystem.state == BattleState.ENEMYTURN && owner == Owner.ENEMY) //Gegnerische Breitseite
        {
            cardActed = true;
            
            if (cardIngameSlot.enemyArtilleryLine.currentCard != null) //Gegner Kanonier greift spieler Arty Karte an
            { 
                CannonBallAnimation(-900f,0.8f, BroadsideEffects);
                cannoneerAttacked = cardIngameSlot.enemyArtilleryLine.currentCard;
                broadsideDamage = enemyManager.enemyCannonLevel;
            }
            else //Gegner greift Spieler Schiff an
            { 
                CannonBallAnimation(-1100f,0.8f, BroadsideEffects);
                cannoneerAttacked = null;
            }
            
        }
        else
        {
            Debug.LogWarning("Attack failed!");
        }
    }

    private void BroadsideEffects() //Verursacht Schaden, wir über Animation Event aufgerufen
    {
        cannonBallVisualRoot.SetActive(false);

        if (cannoneerAttacked != null) //Wenn Karte Gesetzt -> Schaden wird an Karte gemacht
        {
            damageCounterFolder.SpawnDamageCounter(cannoneerAttacked.rectTransform.position + new Vector3(75,-75,0), broadsideDamage);
            cannoneerAttacked.UpdateCardHealth(broadsideDamage);
        }
        else //Wenn keine Karte, wird entsprechendes Schiff angegriffen
        {
            if (owner == Owner.PLAYER)
            {
                damageCounterFolder.SpawnDamageCounter(enemyManager.enemyHealthText.rectTransform.position + new Vector3(75,-75,0), GameManager.instance.shipCannonLevel+1);
                enemyManager.UpdateEnemyHealth(GameManager.instance.shipCannonLevel+1, false);
            }
            else if (owner == Owner.ENEMY)
            {
                playerManager.UpdateHealth(enemyManager.enemyCannonLevel, false);
                damageCounterFolder.SpawnDamageCounter(playerManager.healthText.rectTransform.position+ new Vector3(75,75,0), cardStats.attack);
            }
        }
    }

    #endregion

    public void DidCardAct()
    {
        //Zeigt ob die Karte schon gehandelt hat & Setzt Rand entsprechned
        if (!cardActed && currentCardMode == CardMode.INPLAY && battleSystem.state == BattleState.PLAYERTURN && owner == Owner.PLAYER)
        {
            hasActedRim.SetActive(true);
        }
        else
        {
            hasActedRim.SetActive(false);
        }
    }

    public void UpdateCardHealth(int damage) //Setzt Kartenleben neu
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
        //Verschiebt Karte in den Ablagestapel
        if (owner == Owner.PLAYER)
        {
            deckManager.discardPile.Add(this);
            
        }
        else if (owner == Owner.ENEMY)
        {
            enemyManager.discardPile.Add(this);
        }
        GetComponent<DeathEffects>()?.TriggerDeathEffect(); //Todeseffekte, wenn vorhanden
        VolumeManager.instance.GetComponent<AudioManager>().PlayCardDeathSound(); //Todessound
        
        ResetCard();
    }

    private void ResetCard() //Für Karten die gestorben sind oder sich zurückziehen
    {
        if (owner == Owner.PLAYER)
        {
            GetComponentInChildren<DragDrop>(true).gameObject.SetActive(true); //Karte kann wieder bewegt werden
            GetComponentInChildren<DragDrop>().foundSlot = false; //Leert Kartenslot
            hasActedRim.SetActive(false);
        }
        else if (owner == Owner.ENEMY)
        {
            enemyManager.UpdateEnemyUI();
            cardBG.SetActive(true);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        
        handCard.transform.localScale = new Vector3(1f, 1f, 1f); //Setzt Kartengrösse zurück
        currentCardMode = CardMode.INDECK; //Setzt Karte in einen passiven Modus
        currentHealth = cardStats.defense; //Setzt Leben der Karte zurück
        cardDisplay.SetUpCardUI();  //Setzt UI Werte der Karte
        deckManager.HideDisplayCard(); //Vorschau Karte wird ausgeschaltet
        cardIngameSlot.currentCard = null; //Leert Kartenreferenz auf dem Slot //BUG: cardIngameSlot manchmal null
        cardIngameSlot = null; //Leet verbundener Slot
        
        foreach (DiesWhenAlone var in FindObjectsOfType<DiesWhenAlone>()) //Karten mit deisem Effekt überprüfen ob sie alleine sind
        {
            if (var.GetComponent<CardManager>().currentCardMode == CardMode.INPLAY)
            {
                var.CheckIfAlone();
            }
        }
        
        handCard.SetActive(true); //Setzt visuell Handkarte aktiv
        inGameCard.SetActive(false); //Setzt ingmae Karte pasiv
        gameObject.SetActive(false); //Setzt Karte an sich passiv
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
    
    private void CannonBallAnimation(float distance, float duration, TweenCallback onEnd) //Animation der Broadside
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

    public void SmokeParticles()
    {
        ParticleSystem particles = Instantiate(particleSmoke, rectTransform.position, quaternion.identity, damageCounterFolder.transform); //Smoke
        particles.Play();
    }

    #endregion
    
}

public enum Owner //Von wem die Karte genutzt wird
{
    PLAYER, ENEMY
}

public enum CardMode //In welchem Modus sich die Karte befindet
{
    INHAND, INPLAY, INRECRUIT, INDECK, TODISCARD
}
