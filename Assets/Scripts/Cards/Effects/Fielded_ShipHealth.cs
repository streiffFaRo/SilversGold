using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fielded_ShipHealth : MonoBehaviour
{
    
    private CardManager card;
    private PlayerManager playerManager;
    private EnemyManager enemyManager;
    private BattleSystem battleSystem;
    
    private void Start()
    {
        card = GetComponent<CardManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    private void OnEnable()
    {
        BattleSystem.onPlayerTurnEvent += ChangeShipHealthEffect;
        BattleSystem.onEnemyTurnEvent += ChangeShipHealthEffect;

    }
    
    private void OnDisable()
    {
        BattleSystem.onPlayerTurnEvent -= ChangeShipHealthEffect;
        BattleSystem.onEnemyTurnEvent -= ChangeShipHealthEffect;
    }

    public void ChangeShipHealthEffect()
    {
        if (card.currentCardMode == CardMode.INPLAY)
        {
            if (card.owner == Owner.PLAYER && battleSystem.state == BattleState.PLAYERTURN)
            {
                playerManager.UpdateHealth(card.cardStats.para1, card.cardStats.para4);
            }
            else if (card.owner == Owner.ENEMY && battleSystem.state == BattleState.ENEMYTURN)
            {
                enemyManager.UpdateEnemyHealth(card.cardStats.para1, card.cardStats.para4);
            }
        }
    }
    
}
