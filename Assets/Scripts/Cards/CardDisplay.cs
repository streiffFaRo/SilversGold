using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour 
{
    //Verantwortlich für das visuelle Darstellen der Karte
    
    public Card card;

    [Header("HandCard")]
    public TextMeshProUGUI handNameText;
    public TextMeshProUGUI handDescriptionText;
    public Image handPositionImage;
    public Texture2D infIcon;
    
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

    public void SetUpCardUI() //Setzt Werte der Karte
    {
        handNameText.text = card.name;
        if (card.description != null)
        {
            handDescriptionText.text = card.description;
        }

        if (card.position == "I")
        {
            Sprite sprite = Sprite.Create(infIcon, new Rect(0,0, infIcon.width, infIcon.height),new Vector2(0.5f, 0.5f));
            handPositionImage.sprite = sprite;
        }
        handArtworkImage.sprite = card.artwork;
        inGameArtworkImage.sprite = card.artwork;
        handCostText.text = card.cost.ToString();
        inGameAttackText.text = card.attack.ToString();
        handAttackText.text = card.attack.ToString();
        inGameDefenseText.text = card.defense.ToString();
        inGameDefenseText.color = Color.white;
        handDefenseText.text = card.defense.ToString();
    }

    public void ShowKeyWordBox() //Zeigt Box mit Hilfstext zum Effekt der Karte
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

    public void HideKeyWordBox() //Versteckt Box mit Hilfstext
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
