using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeathEffects : MonoBehaviour
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
        deckManager = FindObjectOfType<DeckManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
        battleSystem = FindObjectOfType<BattleSystem>();
    }
    

    public void TriggerDeathEffect()
    {
        switch (card.cardStats.para1)
        {
            case 1:
                RecruitCardToDeck();
                break;
            case 2:
                ShuffleCardIntoDeck();
                break;
            case 3:
                DrawCard();
                break;
            case 4:
                AllPlayersDrawCard();
                break;
            case 5:
                ChangeShipHealth();
                break;
            case 6:
                ChangeCommandPower();
                break;
        }
    }

    public void RecruitCardToDeck()
    {
        if (card.owner == Owner.PLAYER)
        {
            for (int i = 0; i < card.cardStats.para2; i++)
            {
                GameObject cardToAdd = Instantiate(deckManager.displayCardPrefab, new Vector3(0, 0, 0),
                    Quaternion.identity, deckManager.deckHolder.transform);
                cardToAdd.GetComponent<CardDisplay>().card = card.cardStats.para5;
                cardToAdd.SetActive(false);
                deckManager.deck.Add(cardToAdd.GetComponent<CardManager>());
            }
        }
        else if (card.owner == Owner.ENEMY)
        {
            for (int i = 0; i < card.cardStats.para2; i++)
            {
                GameObject cardToAdd = Instantiate(enemyManager.displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, enemyManager.enemyDeckHolder.transform);
                cardToAdd.GetComponent<CardDisplay>().card = card.cardStats.para5;
                cardToAdd.SetActive(false);
                enemyManager.deck.Add(cardToAdd.GetComponent<CardManager>());
                enemyManager.UpdateEnemyUI();
            }
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

    public void DrawCard()
    {
        if (card.owner == Owner.PLAYER)
        {
            for (int i = 0; i < card.cardStats.para2; i++)
            {
                deckManager.DrawCards();
            }
        }
        else if (card.owner == Owner.ENEMY)
        {
            for (int i = 0; i < card.cardStats.para2; i++)
            {
                enemyManager.DrawCards();
            }
        }
    }

    public void AllPlayersDrawCard()
    {
        for (int i = 0; i < card.cardStats.para2; i++)
        {
            deckManager.DrawCards();
            enemyManager.DrawCards();
        }
    }

    public void ChangeShipHealth()
    {
        if (card.owner == Owner.PLAYER)
        {
            playerManager.UpdateHealth(card.cardStats.para2, card.cardStats.para4);
        }
        else if (card.owner == Owner.ENEMY)
        {
            enemyManager.UpdateEnemyHealth(card.cardStats.para2, card.cardStats.para4);
        }
    }

    public void ChangeCommandPower()
    {
        if (card.owner == Owner.PLAYER)
        {
            playerManager.commandPowerBonus = card.cardStats.para2;
        }
        else if (card.owner == Owner.ENEMY)
        {
            enemyManager.commandPowerBonus = card.cardStats.para2;
        }
    }

}
