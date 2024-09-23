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

    private void AnalysedDrawCardAction()
    {
        float score;
        if (enemyManager.cardsInHand.Count >= 5)
        {
            score = -10;
        }
        else
        {
            score = enemyManager.cardsInHand.Count * -2;
            score += 10;
            score *= strat.drawMod;
        }
        
        actionIndex++;
        

    }

    private void AnalysePlayCardActions()
    {
        
    }

    private void AnalyseAttackActions()
    {
        
    }

    private void AnalyseRetreatActions()
    {
        
    }

    private void AnalyseBroadsideAction()
    {
        
    }
}
