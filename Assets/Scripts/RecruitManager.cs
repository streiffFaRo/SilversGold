using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RecruitManager : MonoBehaviour
{

    public GameObject displayCardPrefab;
    
    [Header("Card Tiers")]
    public List<Card> tier1Cards = new List<Card>();
    public List<Card> tier2Cards = new List<Card>();
    public List<Card> tier3Cards = new List<Card>();
    private List<Card> selectedCardStack = new List<Card>();
    
    [Header("Recruit Slots")]
    public GameObject recruitCardDisplay;
    

    public void ShowRecruitmentOptions()
    {
        SelectCurrentTier();
        
        if (GameManager.instance.playerDeck.Count <= GameManager.instance.deckCardLimit)
        {
            //TODO Möglichkeit Keine Karte zu nehmen mittels Button
        }

        for (int i = 0; i < 3; i++)
        {
            Card randCard = selectedCardStack[Random.Range(0, selectedCardStack.Count)];
            
            GameObject currentCardPrefab = Instantiate(displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, recruitCardDisplay.transform);
            currentCardPrefab.GetComponent<CardDisplay>().card = randCard;
            currentCardPrefab.GetComponent<CardManager>().handCard.transform.localScale = new Vector3(2f, 2f, 2f);
            currentCardPrefab.GetComponent<CardManager>().currentCardMode = CardMode.INRECRUIT;
            currentCardPrefab.GetComponentInChildren<DragDrop>().GameObject().SetActive(false);
            selectedCardStack.Remove(randCard);
        }
    }
    
    public void SelectCurrentTier()
    {
        switch (GameManager.instance.currentTier)
        {
            case 1:
                selectedCardStack = tier1Cards;
                break;
            case 2:
                selectedCardStack = tier2Cards;
                break;
            case 3:
                selectedCardStack = tier3Cards;
                break;
        }
    }

    public void CardChoosen(Card choosenCard)
    {
        if (GameManager.instance.playerDeck.Count <= GameManager.instance.deckCardLimit)
        {
            GameManager.instance.playerDeck.Add(choosenCard);
        }
        else
        {
            Debug.LogWarning("Eine Karte muss über die Planke!");
            //TODO Volle Deckansicht + Karte bestimmen die zerstört wird + Animation :-)
        }
        

        foreach (Transform cardToClear in recruitCardDisplay.GetComponentInChildren<Transform>())
        {
            Destroy(cardToClear.GameObject());
        }
        
    }




}
