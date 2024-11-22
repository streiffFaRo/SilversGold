using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PresentDeck : MonoBehaviour
{
    //Verantwortlich dem Spieler das Deck zu zeigen
    
    [Header("Struktur")]
    public GameObject deckPresenter;
    public GameObject firstRow;
    public GameObject secondRow;
    public GameObject thirdRow;
    public GameObject cardToAddSlot;
    public GameObject discardCardInfoText;
    public Button continueButton;
    public Button inspectShipButton;
    public GameObject bootyUI;

    [Header("Variablen")]
    public List<Card> deckToPrepare = new List<Card>();
    public GameObject displayCardPrefab;
    private int cardsForEachRow;
    private Card cardToAdd;

    private void Start()
    {
        deckToPrepare = GameManager.instance.playerDeck;
    }

    public void DiscardToHoldDeckLimit(Card cardToAddToDeck)
    {
        cardToAdd = cardToAddToDeck;
        ShowDeckPresenter();
    }
    
    public void ShowDeckPresenter()
    {
        deckPresenter.SetActive(true);
        bootyUI.SetActive(true);
        SetUpPresentation();
    }

    private void SetUpPresentation()
    {
        StartCoroutine(InitiateDeck());

        if (cardToAdd == null)
        {
            StartCoroutine(ShowButtons());
        }
        else
        {
            discardCardInfoText.SetActive(true);
            cardToAddSlot.SetActive(true);
            GameObject currentCardPrefab = Instantiate(displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, cardToAddSlot.transform);
            currentCardPrefab.GetComponent<CardDisplay>().card = cardToAdd;
            currentCardPrefab.GetComponentInChildren<DragDrop>().GameObject().SetActive(false);
        }
    }

    private IEnumerator InitiateDeck()
    {
        firstRow.AddComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
        secondRow.AddComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
        thirdRow.AddComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
        
        foreach (Card card in deckToPrepare)
        {

            Transform slot = FindSlot();
            
            GameObject currentCardPrefab = Instantiate(displayCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, slot);
            currentCardPrefab.GetComponent<CardDisplay>().card = card;
            currentCardPrefab.GetComponentInChildren<DragDrop>().GameObject().SetActive(false);
            currentCardPrefab.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            if (cardToAdd != null)
            {
                currentCardPrefab.GetComponent<CardManager>().currentCardMode = CardMode.TODISCARD;
            }
        }

        yield return new WaitForSeconds(0.01f);
        Destroy(firstRow.GetComponent<HorizontalLayoutGroup>());
        Destroy(secondRow.GetComponent<HorizontalLayoutGroup>());
        Destroy(thirdRow.GetComponent<HorizontalLayoutGroup>());
    }

    private Transform FindSlot()
    {
        int deckSize = deckToPrepare.Count;
        cardsForEachRow = deckSize / 3;
        
        if (firstRow.transform.childCount < cardsForEachRow)
        {
            return firstRow.transform;
        }
        else if (secondRow.transform.childCount < cardsForEachRow)
        {
            return secondRow.transform;
        }
        else
        {
            return thirdRow.transform;
        }
    }

    public void DiscardCard(Card cardToDiscard)
    {
        GameManager.instance.playerDeck.Remove(cardToDiscard);
        GameManager.instance.playerDeck.Add(cardToAdd);
        cardToAdd = null;
        discardCardInfoText.SetActive(false);
        cardToAddSlot.SetActive(false);
        ClearPresentation();
        Invoke("SetUpPresentation", 0.001f);
    }

    private void ClearPresentation()
    {
        foreach (Transform cardToClear in firstRow.GetComponentInChildren<Transform>())
        {
            Destroy(cardToClear.GameObject());
        }
        foreach (Transform cardToClear in secondRow.GetComponentInChildren<Transform>())
        {
            Destroy(cardToClear.GameObject());
        }
        foreach (Transform cardToClear in thirdRow.GetComponentInChildren<Transform>())
        {
            Destroy(cardToClear.GameObject());
        }
    }

    public IEnumerator ShowButtons()
    {
        yield return new WaitForSeconds(2f);
        if (GameManager.instance.currentLevel != 1)
        {
            continueButton.gameObject.SetActive(true);
        }
        inspectShipButton.gameObject.SetActive(true);
    }

    public void LoadShipScene()
    {
        SceneManager.LoadScene("Scene_Ship");
    }

    public void LoadLogBookScene()
    {
        SceneManager.LoadScene("Scene_Log");
    }

}
