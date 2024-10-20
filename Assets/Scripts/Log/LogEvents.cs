using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogEvents : MonoBehaviour
{

    public GameObject cardToPresentSlot;
    public GameObject displayCardPrefab;
    public Card whaler;

    private Card cardAtRisk;

    public void PresentCard(Card cardToPresent)
    {
        cardToPresentSlot.SetActive(true);
        GameObject currentCardPrefab = Instantiate(displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, cardToPresentSlot.transform);
        currentCardPrefab.GetComponent<CardDisplay>().card = cardToPresent;
        currentCardPrefab.GetComponentInChildren<DragDrop>().GameObject().SetActive(false);
    }

    //Ink Events:
    public void Booty(int amount)
    {
        GameManager.instance.booty += amount;
        FindObjectOfType<BootyUI>().UpdateBootyUI();
        VolumeManager.instance.GetComponent<AudioManager>().PlayBootySound();
    }

    public void Whaler()
    {
        //TODO Check if player has 50 Booty! -> Sonst Neuer Text in ink Story
        Booty(-50);
        GameManager.instance.playerDeck.Add(whaler);
    }

    public void Raid()
    {
        Card card = GameManager.instance.playerDeck[Random.Range(0, GameManager.instance.playerDeck.Count)];
        
        GameManager.instance.playerDeck.Remove(card);
        PresentCard(card);
    }

    public void ChooseRdmCard()
    {
        Card card = GameManager.instance.playerDeck[Random.Range(0, GameManager.instance.playerDeck.Count)];
        cardAtRisk = card;
        PresentCard(card);
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

    public void ShipHealthChange(int amount)
    {
        GameManager.instance.startShipHealth += amount;
    }
    
    public void EndCurrentDay()
    {
        cardToPresentSlot.SetActive(false);
        GameManager.instance.UpdateLevel();
        SceneManager.LoadScene("Scene_Content");
    }

}
