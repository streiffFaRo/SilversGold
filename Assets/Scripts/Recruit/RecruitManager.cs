using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Booty Ranges")]
    public Vector2Int bootyRangeTier1;
    public Vector2Int bootyRangeTier2;
    public Vector2Int bootyRangeTier3;

    [Header("BootyUI")]
    public GameObject bootyRewardUI;
    public TextMeshProUGUI bootyCountUI;
    
    [Header("Recruit Slots")]
    public GameObject recruitCardDisplay;
    public GameObject normalInfoText;
    public GameObject deckFullInfoText;
    public GameObject noRecruitmentButton;

    [Header("Scripts")]
    public PresentDeck presentDeck;

    public void ShowBootyReward()
    {
        //TODO Animation & Sound
        int bootyGainedThisLevel = CalculateBootyReward();
        GameManager.instance.booty += bootyGainedThisLevel;
        bootyRewardUI.SetActive(true);
        bootyCountUI.text = bootyGainedThisLevel.ToString();
    }

    public void HideBootyReward()
    {
        bootyRewardUI.SetActive(false);
    }

    private int CalculateBootyReward()
    {
        int bootyGained = 0;
        
        switch (GameManager.instance.currentTier)
        {
            case 1:
                bootyGained = Random.Range(bootyRangeTier1.x, bootyRangeTier1.y);
                return bootyGained;
            case 2:
                bootyGained = Random.Range(bootyRangeTier2.x, bootyRangeTier2.y);
                return bootyGained;
            case 3:
                bootyGained = Random.Range(bootyRangeTier3.x, bootyRangeTier3.y);
                return bootyGained;
            default:
                return 0;
        }
    }

    public void ShowRecruitmentOptions()
    {
        //TODO Animation & Sound
        SelectCurrentTier();
        
        if (GameManager.instance.playerDeck.Count+1 <= GameManager.instance.deckCardLimit)
        {
            normalInfoText.SetActive(true);
        }
        else
        {
            deckFullInfoText.SetActive(true);
            noRecruitmentButton.SetActive(true);
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
        if (GameManager.instance.playerDeck.Count+1 <= GameManager.instance.deckCardLimit)
        {
            GameManager.instance.playerDeck.Add(choosenCard);
            presentDeck.ShowDeckPresenter();
        }
        else
        {
            Debug.LogWarning("Eine Karte muss über die Planke!");
            presentDeck.DiscardToHoldDeckLimit(choosenCard);
            //TODO Animation :-)
        }
        VolumeManager.instance.GetComponent<AudioManager>().PlayCardDrawSound();
        
        deckFullInfoText.SetActive(false);
        noRecruitmentButton.SetActive(false);
        normalInfoText.SetActive(false);
        
        foreach (Transform cardToClear in recruitCardDisplay.GetComponentInChildren<Transform>())
        {
            Destroy(cardToClear.GameObject());
        }
    }

    public void ChooseNoCard()
    {
        presentDeck.ShowDeckPresenter();
        
        deckFullInfoText.SetActive(false);
        noRecruitmentButton.SetActive(false);
        normalInfoText.SetActive(false);
        foreach (Transform cardToClear in recruitCardDisplay.GetComponentInChildren<Transform>())
        {
            Destroy(cardToClear.GameObject());
        }
    }
}
