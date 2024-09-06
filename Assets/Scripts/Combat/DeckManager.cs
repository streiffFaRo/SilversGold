using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    public List<CardManager> deck = new List<CardManager>();
    public List<CardManager> discardPile = new List<CardManager>();
    
    public Transform[] cardSlots;
    public bool[] availableCardSlots;

    public TextMeshProUGUI deckSizeText;
    public TextMeshProUGUI discardPileText;
    public TextMeshProUGUI commandPowerText;

    public int currentCommandPower;
    
    private void Update()
    {
        deckSizeText.text = deck.Count.ToString();
        discardPileText.text = discardPile.Count.ToString();
    }

    private void Start()
    {
        ResetCommandPower();
    }

    public void DrawCards()
    {
        if (deck.Count >= 1)
        {
            CardManager randCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;
                    
                    randCard.transform.position = cardSlots[i].position;
                    randCard.hasBeenPlayed = false;
                    
                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    return;
                }
            }
        }
    }
    
    public void Shuffle()
    {
        if (discardPile.Count >=1)
        {
            ResetCommandPower();
            foreach (CardManager cM in discardPile)
            {
                deck.Add(cM);
            }
            discardPile.Clear();
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Scene_Content");
    }

    public void ResetCommandPower()
    {
        currentCommandPower = GameManager.instance.startCommandPower;
        commandPowerText.text = currentCommandPower.ToString();
    }    
    
    public void UpdateCommandPower(int commandPowerCost)
    {
        currentCommandPower -= commandPowerCost;
        commandPowerText.text = currentCommandPower.ToString();
    }
}
