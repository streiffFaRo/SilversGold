using UnityEngine;
using Random = UnityEngine.Random;

public class DeathEffects : MonoBehaviour 
{
    //Verantwortlich für Todeseffekte der Karten
    
    //Private Scripts
    private CardManager card;
    private PlayerManager playerManager;
    private DeckManager deckManager;
    private EnemyManager enemyManager;
    
    private void Start()
    {
        card = GetComponent<CardManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        deckManager = FindObjectOfType<DeckManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    public void TriggerDeathEffect() //Führt gewünschten Effekt aus
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

    public void RecruitCardToDeck() //Fügt dem Deck X Karten hinzu
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

    public void ShuffleCardIntoDeck() //Mischt X tote Karten wieder ins Deck
    {
        if (card.owner == Owner.PLAYER)
        {
            for (int i = 0; i < card.cardStats.para2; i++)
            {
                CardManager randCard = deckManager.discardPile[Random.Range(0, deckManager.discardPile.Count)];
                deckManager.deck.Add(randCard);
                deckManager.discardPile.Remove(randCard);
            }
        }
        else if (card.owner == Owner.ENEMY)
        {
            for (int i = 0; i < card.cardStats.para2; i++)
            {
                CardManager randCard = enemyManager.discardPile[Random.Range(0, enemyManager.discardPile.Count)];
                enemyManager.deck.Add(randCard);
                enemyManager.discardPile.Remove(randCard);
                enemyManager.UpdateEnemyUI();
            }
        }
    }

    public void DrawCard() //Zieht X Karten
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

    public void AllPlayersDrawCard() //Zieht X Karten für beide Spieler
    {
        for (int i = 0; i < card.cardStats.para2; i++)
        {
            deckManager.DrawCards();
            enemyManager.DrawCards();
        }
    }

    public void ChangeShipHealth() //Heilt oder schädigt das eigene Schiff für X
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

    public void ChangeCommandPower() //Gibt nächsten Zug X CommandPower mehr oder weniger
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
