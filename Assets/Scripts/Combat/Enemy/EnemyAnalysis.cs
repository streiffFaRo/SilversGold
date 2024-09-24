using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAnalysis : MonoBehaviour
{
    // Verantwortlich für Spielanalyse
    
    public EnemyManager enemyManager;
    public EnemyActionExecuter enemyActionExecuter;
    public int actionIndex;
    public int cp;
    public Strategy strat;
    public TreeNode treeNode;
    

    public void AnalysePossibleActions()
    {
        strat = enemyManager.strategy;
        cp = enemyManager.enemyCurrentCommandPower;
        
        AnalysedDrawCardAction();
        AnalysePlayCardActions();
        AnalyseAttackActions();
        AnalyseRetreatActions();
        AnalyseBroadsideAction();
        
    }

    private void AnalysedDrawCardAction() //Werte von -10 - 32,5
    {
        if (enemyManager.enemyCurrentCommandPower >= 2)
        {
            float score;
            switch (enemyManager.cardsInHand.Count)
            {
                case 0:
                    score = 25 * strat.drawMod;
                    break;
                case 1:
                    score = 15 * strat.drawMod;
                    break;
                case 2:
                    score = 8 * strat.drawMod;
                    break;
                case 3:
                    score = 4 * strat.drawMod;
                    break;
                case 4:
                    score = 1 * strat.drawMod;
                    break;
                case 5:
                    score = -10;
                    break;
            }
            //TODO Score übermitteln
            actionIndex++;
        }
    }

    private void AnalysePlayCardActions()
    {
        foreach (CardManager cardToPlay in enemyManager.cardsInHand)
        {
            if (cardToPlay.cardStats.cost <= enemyManager.enemyCurrentCommandPower)
            {
                
            }
        }
    }

    private void AnalyseAttackActions() //Werte zwischen -10 ~ 34
    {
        float score = 2;
        
        if (enemyManager.enemyCurrentCommandPower >= 1)
        {
            foreach (CardManager cardToAttackWith in FindObjectsOfType<CardManager>())
            {
                if (cardToAttackWith.owner == Owner.ENEMY && !cardToAttackWith.cardActed && cardToAttackWith.currentCardMode == CardMode.INPLAY)
                {
                    score = score + cardToAttackWith.cardStats.pointsAttack * strat.attackMod + cardToAttackWith.cardIngameSlot.enemyInfantryLine.currentCard.cardStats.dangerLevel;
                    if (score <= 0)
                    {
                        score = -10;
                    }
                    //TODO Score übermitteln
                    actionIndex++;
                }
            }
        }
    }

    private void AnalyseRetreatActions() //Werte von -10 - 24
    {
        float score = 0;
        
        if (enemyManager.enemyCurrentCommandPower >= 1)
        {
            foreach (CardManager cardToRetreat in FindObjectsOfType<CardManager>())
            {
                if (cardToRetreat.owner == Owner.ENEMY && !cardToRetreat.cardActed && cardToRetreat.currentCardMode == CardMode.INPLAY)
                {
                    score = score + cardToRetreat.cardStats.pointsRetreat * strat.retreatMod;
                    if (score <= 0)
                    {
                        score = -10;
                    }
                    //TODO Score übermitteln
                    actionIndex++;
                }
            }
        }
    }

    private void AnalyseBroadsideAction()
    {
        if (enemyManager.enemyCurrentCommandPower >= 1)
        {
            int cannoneerCount = 0;
            float score = 0;
            
            foreach (CardManager cardToCheck in FindObjectsOfType<CardManager>())
            {
                if (cardToCheck.owner ==  Owner.ENEMY && cardToCheck.cardStats.isCannoneer && !cardToCheck.cardActed && cardToCheck.currentCardMode == CardMode.INPLAY)
                {
                    cannoneerCount++;
                }
            }
            score = cannoneerCount * 3;
            score = score * strat.broadsideMod * enemyManager.enemyCannonLevel * 0.5f;
            if (score == 0)
            {
                score = -10;
            }
            
            //TODO Score übermitteln
            actionIndex++;
        }
    }
}
