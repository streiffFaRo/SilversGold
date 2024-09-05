using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI positionText;

    public Image artworkImage;
    
    public TextMeshProUGUI costText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    
    
    void Start()
    {
        nameText.text = card.name;
        if (card.description != null)
        {
            descriptionText.text = card.description;
        }
        positionText.text = card.position;
        artworkImage.sprite = card.artwork;
        costText.text = card.cost.ToString();
        attackText.text = card.attack.ToString();
        defenseText.text = card.defense.ToString();
    }

    
}
