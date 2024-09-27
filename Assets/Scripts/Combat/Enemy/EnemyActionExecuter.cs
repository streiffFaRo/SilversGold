using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionExecuter : MonoBehaviour
{
    //verantwortlich für Strategiefinmdung und Ausführung

    public EnemyManager enemyManager; //Referenz zum EnemyManager (Deck, Handkarten, Leben, CommandPower etc)
    public List<CardIngameSlot> infSlots = new List<CardIngameSlot>(); //Kampfslots (Inf) auf Seiten des Enemy
    public List<CardIngameSlot> artySlots = new List<CardIngameSlot>(); //Kampfslots (Arty) auf Seiten des Enemy
    public int tries; //Versuche Karten zu platzieren
    

    #region Debug/Rdm Actions
    //Ausführuhng

    public void PlayRdmCard()
    {
        //Funktion nur zum testen (benötigt noch Commandpower Abfrage bei mehr als 1 gespielten Karte pro Zug)
        CardManager randCard = enemyManager.cardsInHand[Random.Range(0, enemyManager.cardsInHand.Count)];

        if (tries <= 100)
        {
            if (randCard.cardStats.position == "I")
            {
                CardIngameSlot randSlot = infSlots[Random.Range(0, infSlots.Count)];
                if (randSlot.currentCard == null)
                {
                    randSlot.EnemyCardPlacedOnThisSlot(randCard);
                    randCard.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
                else
                {
                    Debug.LogWarning("Kein offner Slot gefunden");
                    tries++;
                    PlayRdmCard();
                }
            }
            else if (randCard.cardStats.position == "A")
            {
                CardIngameSlot randSlot = artySlots[Random.Range(0, artySlots.Count)];
                if (randSlot.currentCard == null)
                {
                    randSlot.EnemyCardPlacedOnThisSlot(randCard);
                    randCard.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
                else
                {
                    Debug.LogWarning("Kein offner Slot gefunden");
                    tries++;
                    PlayRdmCard();
                }
            }
            else
            {
                Debug.LogError("Card has no assigned position!");
            }
        }
        
    }

    public void LetAllEnemysAttack()
    {
        CardManager[] cardsInPlay = FindObjectsOfType<CardManager>();
        
        foreach (CardManager cardToAttackWith in cardsInPlay)
        {
            if (cardToAttackWith.owner == Owner.ENEMY && cardToAttackWith.currentCardMode == CardMode.INPLAY && cardToAttackWith.cardActed == false && cardToAttackWith.cardStats.attack > 0)
            {
                if (enemyManager.enemyCurrentCommandPower > 0)
                {
                    cardToAttackWith.Attack();
                }
                else
                {
                    Debug.LogWarning("Enemy hat keine Commandpower mehr");
                }
                
            }
        }
    }
    

    #endregion

    #region ExecuteAction

    public void ExecuteAction(int actionIndex)
    {
        
        int executionActionIndex = 1;

        if (enemyManager.enemyCurrentCommandPower >= 2)
        {
            if (executionActionIndex == actionIndex)
            {
                DrawCard();
            }
            else
            {
                executionActionIndex++;
            }
        }

        if (enemyManager.enemyCurrentCommandPower >= 1)
        {
            if (executionActionIndex == actionIndex)
            {
                Broadside();
            }
            else
            {
                executionActionIndex++;
            }
        }
        
        foreach (CardManager cardToPlay in enemyManager.cardsInHand)
        {
            if (cardToPlay.cardStats.cost <= enemyManager.enemyCurrentCommandPower)
            {
                    
                
                if (cardToPlay.cardStats.position == "I")
                {
                    foreach (CardIngameSlot slot in infSlots)
                    {
                        if (slot.currentCard == null)
                        {
                            if (executionActionIndex == actionIndex)
                            {
                                PlayCard(cardToPlay, slot);
                            }
                            else
                            {
                                executionActionIndex++;
                            }
                        }
                    }
                }
                else if (cardToPlay.cardStats.position == "A")
                {
                    foreach (CardIngameSlot slot in artySlots)
                    {
                        if (slot.currentCard == null)
                        {
                            if (executionActionIndex == actionIndex)
                            {
                                PlayCard(cardToPlay, slot);
                            }
                            else
                            {
                                executionActionIndex++;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("Problem with Cardposition!");
                }
            }
        }

        if (enemyManager.enemyCurrentCommandPower >= 1)
        {
            foreach (CardManager cardToAttackWith in FindObjectsOfType<CardManager>())
            {
                if (cardToAttackWith.owner == Owner.ENEMY && !cardToAttackWith.cardActed && cardToAttackWith.currentCardMode == CardMode.INPLAY)
                {
                    if (executionActionIndex == actionIndex)
                    {
                        AttackWithCard(cardToAttackWith);
                    }
                    else
                    {
                        executionActionIndex++;
                    }
                }
            }
        }

        if (enemyManager.enemyCurrentCommandPower >= 1)
        {
            foreach (CardManager cardToRetreat in FindObjectsOfType<CardManager>())
            {
                if (cardToRetreat.owner == Owner.ENEMY && !cardToRetreat.cardActed && cardToRetreat.currentCardMode == CardMode.INPLAY)
                {
                    if (executionActionIndex == actionIndex)
                    {
                        RetreatCard(cardToRetreat);
                    }
                    else
                    {
                        executionActionIndex++;
                    }
                    
                }
            }
        }
        
        if (executionActionIndex >= actionIndex)
        {
            Debug.LogWarning("Fehler bei Ablgeich der ActionIndexes, kein Play gefunden!");
        }
    }

    #endregion
    
    #region PossibleActions

    public void DrawCard()
    {
        enemyManager.BuyCard();
    }
    
    public void Broadside()
    {
        if (enemyManager.battleSystem.state == BattleState.ENEMYTURN && enemyManager.enemyCurrentCommandPower > 0)
        {
            foreach (CardManager card in FindObjectsOfType<CardManager>())
            { 
                if (card.owner == Owner.ENEMY && card.currentCardMode == CardMode.INPLAY && !card.cardActed && card.cardStats.isCannoneer) 
                {
                    card.Broadside();
                }
            }
            enemyManager.UpdateEnemyCommandPower(1);
        }
    }
    
    public void PlayCard(CardManager cardToPlay, CardIngameSlot slot)
    {
        slot.EnemyCardPlacedOnThisSlot(cardToPlay);
        cardToPlay.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void AttackWithCard(CardManager cardToAttackWith)
    {
        cardToAttackWith.Attack();
    }

    public void RetreatCard(CardManager cardToRetreat)
    {
        cardToRetreat.Retreat();
    }

    #endregion
}