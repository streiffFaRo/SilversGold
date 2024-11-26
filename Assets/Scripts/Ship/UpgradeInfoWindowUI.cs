using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (upgradeCostWhenBought != 0)
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
                VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
                Debug.LogWarning("Nicht genug Beute!");
            }
        }
        else
        {
            VolumeManager.instance.GetComponent<AudioManager>().PlayDenySound();
        }
    }

    public void SwitchSceneToContent()
    {
        VolumeManager.instance.GetComponent<AudioManager>().PlayPlatzHalterTeller();
        SceneManager.LoadScene("Scene_Log");
    }

}
