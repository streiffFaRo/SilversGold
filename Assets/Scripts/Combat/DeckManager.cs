using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    //Verantwortlich für Deck, Ablagestapel, ziehen von Karten und managen der Handkarten des Spielers

    [Header("General")]
    public GameObject displayCardPrefab;
    public GameObject deckHolder; //Alle Karten des Spielerdecks werden seine Kinder für die Ordnung
    public List<Card> deckToPrepare = new List<Card>();
    public List<CardManager> deck = new List<CardManager>();
    public List<CardManager> discardPile = new List<CardManager>();
    public List<CardManager> cardsInHand = new List<CardManager>();
    public int currentFatigueDamage = 1;
    
    [Header("Animation")]
    public GameObject infoBanner;
    public Animator deckAnimator;
    public Animator handCardSlotAnimator;
    
    [Header("Display")]
    public GameObject displayStand;
    private CardDisplay displayUI;
    
    [HideInInspector]public CardManager[] allPresentCards;
    public Transform[] cardSlots;
    public bool[] availableCardSlots;
    
    [Header("Scripts")]
    public BattleSystem battleSystem;
    public PlayerManager playerManager;
    public DamageCounterFolder damageCounterFolder;
    
    private void Start()
    {
        InitilizeDeck();
        
        //Vorschaukarte wenn Spieler über gespielte Karte hovered
        GameObject displayObj = Instantiate(displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, displayStand.transform);
        displayObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        displayUI = displayStand.GetComponentInChildren<CardDisplay>();
        
        currentFatigueDamage = 1;
    }

    private void InitilizeDeck() //Baut Spielerdeck
    {
        deckToPrepare = GameManager.instance.playerDeck;
        
        foreach (Card card in deckToPrepare)
        {
            GameObject currentCardPrefab = Instantiate(displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, deckHolder.transform);
            currentCardPrefab.GetComponent<CardDisplay>().card = card;
            currentCardPrefab.SetActive(false);
            deck.Add(currentCardPrefab.GetComponent<CardManager>());
        }
    }

    private void Update()
    {
        //TODO nachfolgenden Zeilen neu eingliedern, ausserhalb von Update
        playerManager.deckSizeText.text = deck.Count.ToString();
        playerManager.discardPileText.text = discardPile.Count.ToString();
        if (deck.Count <= 0)
        {
            playerManager.deckSizeText.color = Color.red;
        }
        else
        {
            playerManager.deckSizeText.color = Color.white;
        }
    }
    
    public void DrawCards()
    {
        if (deck.Count >= 1)
        {
            CardManager randCard = deck[Random.Range(0, deck.Count)];

            if (cardsInHand.Count >= 5) //Bei voller Hand (5Karten) wird die nächste Verbrannt
            {
                BurnTopDeckCard(randCard);
            }
            else
            {
                if (randCard.currentCardMode == CardMode.INDECK) //Schliesst mögliche Fehler aus
                {
                    for (int i = 0; i < availableCardSlots.Length; i++)
                    {
                        if (availableCardSlots[i])
                        {
                            randCard.gameObject.SetActive(true);
                            randCard.handIndex = i;
                    
                            randCard.transform.position = cardSlots[i].position;
                            randCard.currentCardMode = CardMode.INHAND;
                        
                            VolumeManager.instance.GetComponent<AudioManager>().PlayCardDrawSound();
                            availableCardSlots[i] = false;
                            deck.Remove(randCard);
                            cardsInHand.Add(randCard);
                            return;
                        }
                    }
                }
                else //Bei Fehler wird versucht eine andere Karte zu ziehen
                {
                    DrawCards();
                }
            }
        }
        else
        {
            Fatigue();
        }
    }

    public void BurnTopDeckCard(CardManager cardToBurn)
    {
        deck.Remove(cardToBurn);
        discardPile.Add(cardToBurn);
        VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
        StartCoroutine(BurnTopDeckCardBanner());
    }

    public IEnumerator BurnTopDeckCardBanner() //Burned Card Info an Spieler
    {
        infoBanner.SetActive(true);
        infoBanner.GetComponentInChildren<TextMeshProUGUI>().text = "Hand full; Card lost!";
        yield return new WaitForSeconds(2f);
        infoBanner.SetActive(false);
    }

    public void Fatigue()
    {
        playerManager.UpdateHealth(currentFatigueDamage, false);
        damageCounterFolder.SpawnDamageCounter(playerManager.healthText.rectTransform.position+ new Vector3(75,75,0), currentFatigueDamage);
        Debug.LogWarning("You fatigued for " + currentFatigueDamage);
        VolumeManager.instance.GetComponent<AudioManager>().PlayShipHitSound();
        StartCoroutine(FatigueBanner());
        currentFatigueDamage++;
    }

    public IEnumerator FatigueBanner() //Fatigue Info an Spieler
    {
        infoBanner.SetActive(true);
        infoBanner.GetComponentInChildren<TextMeshProUGUI>().text = "Deck empty! Ship explodes!";
        yield return new WaitForSeconds(2f);
        infoBanner.SetActive(false);
    }

    public void ButtonDrawsCards()
    {
        if (playerManager.currentCommandPower >= 2)
        {
            if (cardsInHand.Count < 5)
            {
                if (deck.Count > 0)
                {
                    playerManager.UpdateCommandPower(2);
                    DrawCards();
                    VolumeManager.instance.GetComponent<AudioManager>().PlayButtonPressSound();
                }
                else
                {
                    Debug.LogWarning("Deck leer!");
                    VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
                    deckAnimator.SetTrigger("trigger_warn");
                }
            }
            else
            {
                Debug.LogWarning("Hand voll!");
                VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
                handCardSlotAnimator.SetTrigger("trigger_warn");
            }
        }
        else
        {
            Debug.LogWarning("Zu wenig CommandPower");
            VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
            playerManager.drawCardButtonAnimator.SetTrigger("trigger_warn");
            playerManager.commandPowerAnimator.SetTrigger("trigger_commandpower_warn");
        }
    }

    public void Shuffle() //Für Testen, Mischt alle Karten vom Ablagestapel ins Spielerdeck
    {
        if (discardPile.Count >=1 && battleSystem.state == BattleState.PLAYERTURN)
        {
            playerManager.RefreshCommandPower();
            foreach (CardManager cM in discardPile)
            {
                deck.Add(cM);
            }
            discardPile.Clear();
        }
    }

    public void Broadside()
    {
        StartCoroutine(BroadsideCorutine());
    }

    public IEnumerator BroadsideCorutine()
    {
        if (battleSystem.state == BattleState.PLAYERTURN && playerManager.currentCommandPower >= 2)
        {
            List<CardManager> playerArtyCards = new List<CardManager>();
            
            foreach (CardManager card in FindObjectsOfType<CardManager>())
            {
                if (card.owner == Owner.PLAYER && card.currentCardMode == CardMode.INPLAY && !card.cardActed && card.cardStats.keyWordCannoneer)
                {
                    playerArtyCards.Add(card);
                }
            }

            if (playerArtyCards.Count > 0)
            {
                foreach (CardManager card in playerArtyCards)
                { 
                    card.Broadside();
                    yield return new WaitForSeconds(0.2f);
                }
                playerManager.UpdateCommandPower(2); //Zieht CommandPower beim Spieler ab
            }
            else
            {
                Debug.LogWarning("Keine Arty Einheiten für Breitseite!");
                VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
            }
        }
        else
        {
            VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
            playerManager.broadsideButtonAnimator.SetTrigger("trigger_warn");
            playerManager.commandPowerAnimator.SetTrigger("trigger_commandpower_warn");
        }
        VolumeManager.instance.GetComponent<AudioManager>().PlayButtonPressSound();
    }

    public void EndTurn() //Beendet den Spielerzug
    {
        if (battleSystem.state == BattleState.PLAYERTURN)
        {
            StartCoroutine(battleSystem.EnemyTurn());

            foreach (CardManager card in FindObjectsOfType<CardManager>())
            {
                if (card.currentCardMode == CardMode.INPLAY && card.owner == Owner.PLAYER)
                {
                    card.hasActedRim.SetActive(false);
                }
            }
        }
    }

    public void ShowDisplayCard(CardManager cardManager) //Zeigt Kartenvorschau, einer gespielten Karte über die gehovered wird
    {
        displayStand.SetActive(true);
        displayUI.card = cardManager.cardStats;
        displayUI.SetUpCardUI();
        displayUI.ShowKeyWordBox();
    }

    public void HideDisplayCard() //Versteckt Kartenvorschau
    {
        displayStand.SetActive(false);
        displayUI.HideKeyWordBox();
    }

    public void SetAllOtherButtonsPassive(CardManager targetCardManager) //Versteckt alle Aktionenbuttons ausser dem aktuellen
    {
        if (!targetCardManager.cardActed)
        {
            allPresentCards = FindObjectsOfType<CardManager>();

            foreach (CardManager card in allPresentCards)
            {
                for (int i = 0; i < allPresentCards.Length; i++)
                {
                    if (card.currentCardMode == CardMode.INPLAY && card.owner == Owner.PLAYER)
                    {
                        card.SetButtonsPassive();
                    }
                }
            }
            targetCardManager.SetButtonsActive();
        }
        else
        {
            Debug.LogWarning("Karte hat schon gehandelt");
            targetCardManager.animator.SetTrigger("trigger_acted_warn");
            VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
        }
        
    }

    public void SetAllOtherButtonsPassive() //Versteckt alle Aktionenbuttons
    {
        allPresentCards = FindObjectsOfType<CardManager>();

        foreach (CardManager card in allPresentCards)
        {
            for (int i = 0; i < allPresentCards.Length; i++)
            {
                if (card.currentCardMode == CardMode.INPLAY && card.owner == Owner.PLAYER)
                {
                    card.SetButtonsPassive();
                }
            }
        }
    }
}
