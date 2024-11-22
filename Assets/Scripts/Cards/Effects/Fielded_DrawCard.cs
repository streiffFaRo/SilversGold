using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fielded_DrawCard : MonoBehaviour 
{
    //Verantwortlich für Karten die während Existenz jeden Zug Karten ziehen
    
    //Priavte Scripts
    private CardManager card;
    private DeckManager deckManager;
    private EnemyManager enemyManager;
    private BattleSystem battleSystem;
    
    private void Start()
    {
        card = GetComponent<CardManager>();
        deckManager = FindObjectOfType<DeckManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    private void OnEnable()
    {
        BattleSystem.onPlayerTurnEvent += ChangeCommandPowerEffect;
        BattleSystem.onEnemyTurnEvent += ChangeCommandPowerEffect;

    }
    
    private void OnDisable()
    {
        BattleSystem.onPlayerTurnEvent -= ChangeCommandPowerEffect;
        BattleSystem.onEnemyTurnEvent -= ChangeCommandPowerEffect;
    }

    public void ChangeCommandPowerEffect() //Zieht jeden Zug eine extra Karte
    {
        if (card.currentCardMode == CardMode.INPLAY)
        {
            if (card.owner == Owner.PLAYER && battleSystem.state == BattleState.PLAYERTURN)
            {
                deckManager.DrawCards();
            }
            else if (card.owner == Owner.ENEMY && battleSystem.state == BattleState.ENEMYTURN)
            {
                enemyManager.DrawCards();
            }
        }
    }
}
