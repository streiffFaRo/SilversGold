using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatEffects : MonoBehaviour
{
    private CardManager card;
    private PlayerManager playerManager;
    private DeckManager deckManager;
    private EnemyManager enemyManager;
    private BattleSystem battleSystem;
    
    private void Start()
    {
        card = GetComponent<CardManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    public void TriggerRetreatEffect()
    {
        switch (card.cardStats.para1)
        {
            case 1:
                ShuffleCardIntoDeck();
                break;
            case 2:
                StartCoroutine(DrawCard());
                break;
            case 3:
                ChangeShipHealth();
                break;
            case 4:
                ChangeCommandPower();
                break;
            
        }
    }

    public void ShuffleCardIntoDeck()
    {
        if (card.owner == Owner.PLAYER)
        {
            for (int i = 0; i < card.cardStats.para2; i++)
            {
                CardManager randCard = deckManager.deck[Random.Range(0, deckManager.discardPile.Count)];
                deckManager.deck.Add(randCard);
                deckManager.discardPile.Remove(randCard);

            }
        }
        else if (card.owner == Owner.ENEMY)
        {
            for (int i = 0; i < card.cardStats.para2; i++)
            {
                CardManager randCard = enemyManager.deck[Random.Range(0, enemyManager.discardPile.Count)];
                enemyManager.deck.Add(randCard);
                enemyManager.discardPile.Remove(randCard);
                enemyManager.UpdateEnemyUI();
            }
        }
    }

    public IEnumerator DrawCard()
    {
        if (card.owner == Owner.PLAYER)
        {
            for (int i = 0; i < card.cardStats.para2; i++)
            {
                deckManager.DrawCards();
                yield return new WaitForSeconds(0.5f);
            }
        }
        else if (card.owner == Owner.ENEMY)
        {
            for (int i = 0; i < card.cardStats.para2; i++)
            {
                enemyManager.DrawCards();
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public void ChangeShipHealth()
    {
        if (card.owner == Owner.PLAYER)
        {
            playerManager.UpdateHealth(card.cardStats.para2, true);
        }
        else if (card.owner == Owner.ENEMY)
        {
            enemyManager.UpdateEnemyHealth(card.cardStats.para2, true);
        }
    }

    public void ChangeCommandPower()
    {
        if (card.owner == Owner.PLAYER)
        {
            playerManager.UpdateCommandPower(-1 * card.cardStats.para2);
        }
        else if (card.owner == Owner.ENEMY)
        {
            enemyManager.UpdateEnemyCommandPower(-1* card.cardStats.para2);
        }
    }
    



}
