using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeInfoWindowUI : MonoBehaviour
{
    //Verantwortlich für die Anzeige der Upgradeinfos an den Spieler, Initiator des Upgrades
    
    public TextMeshProUGUI currentEffectUI;
    public TextMeshProUGUI upgradeCostUI;
    public TextMeshProUGUI upgradedEffectUI;

    [Header("Scripts")]
    public BootyUI bootyUI;
    
    //Private Variablen
    private int upgradeCostWhenBought;
    private ShipUpgradeArea areaToUpgradeWhenBought;
    
    
    public void UpdateUpgradeInfoWindowUI(string currentEffect, int upgradeCost, string upgradedEffect, ShipUpgradeArea areaToUpgrade)
    {
        currentEffectUI.text = currentEffect;
        upgradeCostUI.text = upgradeCost.ToString();
        upgradedEffectUI.text = upgradedEffect;

        upgradeCostWhenBought = upgradeCost;
        areaToUpgradeWhenBought = areaToUpgrade;
    }

    public void BuyUpgrade()
    {
        if (upgradeCostWhenBought != null)
        {
            if (GameManager.instance.booty >= upgradeCostWhenBought)
            {
                GameManager.instance.booty -= upgradeCostWhenBought;
                areaToUpgradeWhenBought.Upgrade();
                VolumeManager.instance.GetComponent<AudioManager>().PlayUpgradeSound();
                //TODO Animation
                //TODO Upgrade Sound
                bootyUI.UpdateBootyUI();
            }
            else
            {
                Debug.LogWarning("Nicht genug Beute!");
            }
        }
    }

    public void SwitchSceneToContent()
    {
        VolumeManager.instance.GetComponent<AudioManager>().PlayPlatzHalterTeller();
        SceneManager.LoadScene("Scene_Log");
    }

}
