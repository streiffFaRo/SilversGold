using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEvents : MonoBehaviour
{

    public Card whaler;

    private Card cardAtRisk;
    
    public void Booty(int amount)
    {
        GameManager.instance.booty += amount;
    }

    public void Whaler()
    {
        //TODO Check if player has 50 Booty! -> Sonst Neuer Text in ink Story
        Booty(-50);
        GameManager.instance.playerDeck.Add(whaler);
    }

    public void Raid()
    {
        for (int i = 0; i < 2; i++)
        {
            Card card = GameManager.instance.playerDeck[Random.Range(0, GameManager.instance.playerDeck.Count)];
        
            GameManager.instance.playerDeck.Remove(card);
        }
        //TODO Show Cards that got destroyed
    }

    public void ChooseRdmCard()
    {
        Card card = GameManager.instance.playerDeck[Random.Range(0, GameManager.instance.playerDeck.Count)];
        cardAtRisk = card;
        //TODO Show Card at Risk
    }

    public void Plank()
    {
        GameManager.instance.playerDeck.Remove(cardAtRisk);
        cardAtRisk = null;
    }

    public void CommandPowerChange(int amount)
    {
        GameManager.instance.startCommandPower += amount;
    }
    

}
