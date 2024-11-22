using UnityEngine;

public class RetreatEffects : MonoBehaviour 
{
    //Verantwortlich für Karten mit einem Effekt nachdem sie sich zurückziehen
    
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

    public void TriggerRetreatEffect() //Wählt gewünschten Effekt
    {
        switch (card.cardStats.para1)
        {
            case 1:
                ShuffleCardIntoDeck();
                break;
            case 2:
                DrawCard();
                break;
            case 3:
                ChangeShipHealth();
                break;
            case 4:
                ChangeCommandPower();
                break;
        }
    }

    public void ShuffleCardIntoDeck() //Mischt X tote Karten wieder ins Deck
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

    public void DrawCard() //Zieht X Karten
    {
        if (card.owner == Owner.PLAYER)
        {
            for (int i = 0; i < card.cardStats.para2; i++)
            {
                Debug.Log(i);
                deckManager.DrawCards();
                Debug.Log("Draw done");
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

    public void ChangeShipHealth() //Heilt oder schädigt das eigene Schiff für X
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

    public void ChangeCommandPower() //Gibt diesen Zug X CommandPower mehr oder weniger
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
