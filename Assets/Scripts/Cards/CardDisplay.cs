using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    [Header("HandCard")]
    public TextMeshProUGUI handNameText;
    public TextMeshProUGUI handDescriptionText;
    public TextMeshProUGUI handPositionText;

    public Image handArtworkImage;
    
    public TextMeshProUGUI handCostText;
    public TextMeshProUGUI handAttackText;
    public TextMeshProUGUI handDefenseText;
    
    [Header("IngameCard")]
    public Image inGameArtworkImage;
    
    public TextMeshProUGUI inGameAttackText;
    public TextMeshProUGUI inGameDefenseText;
    
    [Header("KeyWordBoxes")]
    public GameObject keyWordCannoneerBox;
    public GameObject keyWordFieldedBox;
    public GameObject keyWordDeathBox;
    public GameObject keyWordRetreatBox;
    
    //Private
    private List<GameObject> keyWords = new List<GameObject>();

    

    void Start()
    {
        if (card.cardEffect != "" && card.cardEffect != " " && card.cardEffect != null)
        {
            Type effectType = Type.GetType(card.cardEffect);
            gameObject.AddComponent(effectType);
        }
        
        SetUpCardUI();
    }

    public void SetUpCardUI()
    {
        handNameText.text = card.name;
        if (card.description != null)
        {
            handDescriptionText.text = card.description;
        }
        handPositionText.text = card.position;
        handArtworkImage.sprite = card.artwork;
        inGameArtworkImage.sprite = card.artwork;
        handCostText.text = card.cost.ToString();
        inGameAttackText.text = card.attack.ToString();
        handAttackText.text = card.attack.ToString();
        inGameDefenseText.text = card.defense.ToString();
        handDefenseText.text = card.defense.ToString();
    }

    public void ShowKeyWordBox()
    {
        if (GameManager.instance.showKeyWords)
        {
            if (card.keyWordCannoneer)
                keyWords.Add(keyWordCannoneerBox);
        
            if (card.keyWordFielded)
                keyWords.Add(keyWordFieldedBox);
        
            if (card.keyWordDeath)
                keyWords.Add(keyWordDeathBox);
        
            if (card.keyWordRetreat)
                keyWords.Add(keyWordRetreatBox);
        
            if (keyWords.Count > 0)
            {
                foreach (GameObject box in keyWords)
                {
                    box.SetActive(true);
                }
            }
        }
    }

    public void HideKeyWordBox()
    {
        if (keyWords.Count > 0)
        {
            foreach (GameObject box in keyWords)
            {
                box.SetActive(false);
            }
        }
        keyWords.Clear();
    }
}
