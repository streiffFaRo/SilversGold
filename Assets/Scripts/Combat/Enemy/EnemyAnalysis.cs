using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAnalysis : MonoBehaviour
{
    // Verantwortlich für Spielanalyse
    
    public EnemyManager enemyManager;
    public EnemyActionExecuter enemyActionExecuter;
    public int actionIndex;
    public Strategy strat;
    public bool showDebug;
    private List<TreeNode> possiblePlays = new();

    private void Start()
    {
        strat = enemyManager.strategy;
    }

    public void AnalysePossibleActions()
    {
        actionIndex = 1;
        
        AnalysedDrawCardAction();
        AnalyseBroadsideAction();
        AnalysePlayCardActions();
        AnalyseAttackActions();
        AnalyseRetreatActions();
    }

    private void AnalysedDrawCardAction() //Werte von -10 - 32,5
    {
        if (enemyManager.enemyCurrentCommandPower >= 2)
        {
            float score = 0;

            if (enemyManager.deck.Count <= 4) //Zieht keine Karten mehr wenn in Fatiguenähe
            {
                score = -10;
            }
            else
            {
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
            }

            TreeNode drawCard = new();
            possiblePlays.Add(drawCard);
            drawCard.score = score;
            drawCard.actionIndex = actionIndex;
            drawCard.name = "DrawCard";
            actionIndex++;
        }
    }
    
    private void AnalyseBroadsideAction()
    {
        if (enemyManager.enemyCurrentCommandPower >= 2)
        {
            int cannoneerCount = 0;
            float score = 0;
            
            foreach (CardManager cardToCheck in FindObjectsOfType<CardManager>())
            {
                if (cardToCheck.owner ==  Owner.ENEMY && cardToCheck.cardStats.keyWordCannoneer && !cardToCheck.cardActed && cardToCheck.currentCardMode == CardMode.INPLAY)
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
            
            TreeNode fireBroadside = new();
            possiblePlays.Add(fireBroadside);
            fireBroadside.score = score;
            fireBroadside.actionIndex = actionIndex;
            fireBroadside.name = "fireBroadSide";
            actionIndex++;
        }
    }

    private void AnalysePlayCardActions()
    {
        foreach (CardManager cardToPlay in enemyManager.cardsInHand)
        {
            if (cardToPlay.cardStats.cost <= enemyManager.enemyCurrentCommandPower)
            {
                float score = 3;
                
                if (cardToPlay.cardStats.position == "I")
                {
                    foreach (CardIngameSlot slot in enemyActionExecuter.infSlots)
                    {
                        if (slot.currentCard == null)
                        {
                            score = score + cardToPlay.cardStats.pointsFielded * strat.playMod;

                            if (slot.enemyInfantryLine == null && slot.enemyArtilleryLine == null)
                            {
                                score *= strat.emptyLaneMod;
                            }
                            
                            TreeNode playInfCard = new();
                            possiblePlays.Add(playInfCard);
                            playInfCard.score = score;
                            playInfCard.actionIndex = actionIndex;
                            playInfCard.name = "PlayInfCard";
                            actionIndex++;
                        }
                    }
                }
                else if (cardToPlay.cardStats.position == "A")
                {
                    foreach (CardIngameSlot slot in enemyActionExecuter.artySlots)
                    {
                        if (slot.currentCard == null)
                        {
                            if (slot.enemyArtilleryLine == null)
                            {
                                score *= strat.emptyLaneMod;
                            }
                            
                            TreeNode playArtyCard = new();
                            possiblePlays.Add(playArtyCard);
                            score *= strat.broadsideMod;
                            playArtyCard.score = score;
                            playArtyCard.actionIndex = actionIndex;
                            playArtyCard.name = "PlayArtyCard";
                            actionIndex++;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Problem with Cardposition!");
                }
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
                    if (cardToAttackWith.cardStats.position == "I")
                    {
                        if (cardToAttackWith.cardIngameSlot.enemyInfantryLine.currentCard != null)
                        {
                            score = score + cardToAttackWith.cardStats.pointsAttack * strat.attackMod + cardToAttackWith.cardIngameSlot.enemyInfantryLine.currentCard.cardStats.dangerLevel;
                        }
                        else if (cardToAttackWith.cardIngameSlot.enemyArtilleryLine.currentCard != null)
                        {
                            score = score + cardToAttackWith.cardStats.pointsAttack * strat.attackMod + cardToAttackWith.cardIngameSlot.enemyArtilleryLine.currentCard.cardStats.dangerLevel;
                        }
                        else
                        {
                            score = score + cardToAttackWith.cardStats.pointsAttack * strat.attackMod + 7; // 7 = Standart direkter Schiffsangriff Wert
                        }
                    }
                    else if (cardToAttackWith.cardStats.position == "A")
                    {
                        if (cardToAttackWith.cardIngameSlot.enemyArtilleryLine.currentCard != null)
                        {
                            score = score + cardToAttackWith.cardStats.pointsAttack * strat.attackMod + cardToAttackWith.cardIngameSlot.enemyArtilleryLine.currentCard.cardStats.dangerLevel;
                        }
                        else
                        {
                            score = score + cardToAttackWith.cardStats.pointsAttack * strat.attackMod + 7; // 7 = Standart direkter Schiffsangriff Wert
                        }
                    }
                    
                    if (score <= 0)
                    {
                        score = -10;
                    }
                    
                    TreeNode attackWithCard = new();
                    possiblePlays.Add(attackWithCard);
                    attackWithCard.score = score;
                    attackWithCard.actionIndex = actionIndex;
                    attackWithCard.name = "AttackWithCard";
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
                    //Berechnung des Scores
                    score +=cardToRetreat.cardStats.pointsRetreat;
                    if (cardToRetreat.currentHealth == 1 && cardToRetreat.cardStats.defense >= 3)
                    {
                        score += 6;
                    }
                    score *= strat.retreatMod;
                    
                    if (score <= 0)
                    {
                        score = -10;
                    }
                    
                    TreeNode retreatCard = new();
                    possiblePlays.Add(retreatCard);
                    retreatCard.score = score;
                    retreatCard.actionIndex = actionIndex;
                    retreatCard.name = "RetreatCard";
                    actionIndex++;
                }
            }
        }
    }

    public void InitiateBestPlay()
    {
        if (showDebug)
        {
            foreach (TreeNode node in possiblePlays)
            {
                 Debug.Log(node.name + " Score: " + node.score + " ActionIndex: " + node.actionIndex);
            }
        }
        
        if (possiblePlays.Count != 0)
        {
            TreeNode bestPlay = possiblePlays.OrderBy(x => x.score).Last();
            if (bestPlay.score > 0)
            {
                if (showDebug)
                {
                    Debug.Log("Gewählter Play: "+bestPlay.name + " Score: " + bestPlay.score + " ActionIndex: " + bestPlay.actionIndex);
                }
                enemyActionExecuter.ExecuteAction(bestPlay.actionIndex);
            }
            else
            {
                StartCoroutine(enemyManager.EndTurn());
                if (showDebug)
                {
                    Debug.LogWarning("No good Plays Possible");
                }
            }
            possiblePlays.Clear();
        }
        else
        {
            StartCoroutine(enemyManager.EndTurn());
            if (showDebug)
            {
                Debug.LogWarning("No Plays Possible");
            }
        }
        
    }

}
