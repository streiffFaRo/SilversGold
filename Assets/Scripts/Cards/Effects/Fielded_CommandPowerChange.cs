using UnityEngine;

public class Fielded_CommandPowerChange : MonoBehaviour 
{
    //Verantwortlich für Karten die während Existenz jeden Zug CommandPower geben oder nehmen

    //Private Scripts
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
        BattleSystem.onPlayerTurnEvent += ChangeCommandPowerEffect;
        BattleSystem.onEnemyTurnEvent += ChangeCommandPowerEffect;

    }
    
    private void OnDisable()
    {
        BattleSystem.onPlayerTurnEvent -= ChangeCommandPowerEffect;
        BattleSystem.onEnemyTurnEvent -= ChangeCommandPowerEffect;
    }

    public void ChangeCommandPowerEffect() //Setzt jeden Zug den CommandPower Bonus oder Malus
    {
        if (card.currentCardMode == CardMode.INPLAY)
        {
            if (card.owner == Owner.PLAYER && battleSystem.state == BattleState.PLAYERTURN)
            {
                playerManager.UpdateCommandPower(-1 * card.cardStats.para1);
            }
            else if (card.owner == Owner.ENEMY && battleSystem.state == BattleState.ENEMYTURN)
            {
                enemyManager.UpdateEnemyCommandPower(-1 * card.cardStats.para1);
            }
        }
    }
    
}
