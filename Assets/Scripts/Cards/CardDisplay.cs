using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    
    void Start()
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

    
}
