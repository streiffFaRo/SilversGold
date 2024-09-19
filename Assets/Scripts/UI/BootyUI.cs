using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BootyUI : MonoBehaviour
{
    //Verantwortlich für die Beuteanzeige
    
    public TextMeshProUGUI bootyNumber;

    private void Start()
    {
        UpdateBootyUI();
    }

    public void UpdateBootyUI()
    {
        bootyNumber.text = GameManager.instance.booty.ToString();
    }
}
