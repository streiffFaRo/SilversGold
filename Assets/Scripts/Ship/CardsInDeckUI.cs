using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardsInDeckUI : MonoBehaviour
{

    public TextMeshProUGUI textUI;

    private void Start()
    {
        textUI.text = GameManager.instance.playerDeck.Count.ToString();
    }
}
