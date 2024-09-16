using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrategy : MonoBehaviour
{
    //verantwortlich für Strategiefinmdung und Ausführung

    public EnemyManager enemyManager;
    public List<CardIngameSlot> infSlots = new List<CardIngameSlot>();
    public List<CardIngameSlot> artySlots = new List<CardIngameSlot>();
    
    #region Strategiefindung

    //Strategiefindung

    #endregion

    #region Ausführung
    //Ausführuhng

    public void PlayRdmCard()
    {
        //Funktion nur zum testen (benötigt noch Commandpower Abfrage bei mehr als 1 gespielten Karte pro Zug)
        CardManager randCard = enemyManager.cardsInHand[Random.Range(0, enemyManager.cardsInHand.Count)];
        
        if (randCard.cardStats.position == "I")
        {
            CardIngameSlot randSlot = infSlots[Random.Range(0, infSlots.Count)];
            if (randSlot.currentCard == null)
            {
                randSlot.EnemyCardPlacedOnThisSlot(randCard);
            }
            else
            {
                Debug.LogWarning("Kein offner Slot gefunden");
                PlayRdmCard();
            }
        }
        else if (randCard.cardStats.position == "A")
        {
            CardIngameSlot randSlot = artySlots[Random.Range(0, artySlots.Count)];
            if (randSlot.currentCard == null)
            {
                randSlot.EnemyCardPlacedOnThisSlot(randCard);
            }
            else
            {
                Debug.LogWarning("Kein offner Slot gefunden");
                PlayRdmCard();
            }
        }
        else
        {
            Debug.LogError("Card has no assigned position!");
        }
        
    }

    public void LetAllEnemysAttack()
    {
        CardManager[] cardsInPlay = FindObjectsOfType<CardManager>();
        
        foreach (CardManager cardToAttackWith in cardsInPlay)
        {
            if (cardToAttackWith.owner == Owner.ENEMY && cardToAttackWith.foundSlot && cardToAttackWith.cardActed == false && cardToAttackWith.cardStats.attack > 0)
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
    
    
}

public enum Strategies
{
    offensive, defensive, focus, massplay
}

//Offensive(Viele Karten im Spiel/Keine mehr auf der Hand): Alle möglichen Angriffe machen
//Defensive(Gegner hat Karten mit grossem Angriff): Mit Karten gegnerische Anfriffe abblocken 
//Focus(DangerLevel einer oder vieler Karten zu hoch): Karten mit dem höchsten DangerLevel töten
//Massplay(Mehr als 3 Karten auf der Hand): Alle möglichen Karten spielen

//Prio: DangerLevelFokus>Angriffe>Rückzüge>Karten spielen