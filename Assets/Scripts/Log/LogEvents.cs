using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEvents : MonoBehaviour
{

    public Card whaler;
    
    public void Booty()
    {
        GameManager.instance.booty += 50;
    }

    public void Whaler()
    {
        //TODO Check if player has 50 Booty! -> Sonst Neuer Text in ink Story
        GameManager.instance.booty -= 50;
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

}
