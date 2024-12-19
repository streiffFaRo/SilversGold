using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipUpgradeArea : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Verantwortlich die Infos über den momentanen Stand der Upgrades, Austausch der Schiffsteile bei upgrades

    public UpgradeType upgradeType;
    public UpgradeInfoWindowUI infoWindow;
    
    [Header("UpgradeInformation")]
    public int currentUpgradeLevel;

    public GameObject silverUpgrade;
    public GameObject goldUpgrade;
    
    public string[] currentEffect;
    public int[] upgradeCosts;
    public string[] upgradeEffect;
    
    public Image image;

    private Tween fadeTween;

    private void Start()
    {
        LoadCurrentUpgradeLevel();
    }

    private void LoadCurrentUpgradeLevel()
    {
        switch (upgradeType)
        {
            case UpgradeType.CANNON:
                currentUpgradeLevel = GameManager.instance.shipCannonLevel;
                break;
            case UpgradeType.CAPTAIN:
                currentUpgradeLevel = GameManager.instance.shipCaptainLevel;
                break;
            case UpgradeType.QUARTERS:
                currentUpgradeLevel = GameManager.instance.shipQuartersLevel;
                break;
            case UpgradeType.HULL:
                currentUpgradeLevel = GameManager.instance.shipHullLevel;
                break;
        }
        
        SetVisuellUpgrade();
    }

    public void SetVisuellUpgrade()
    {
        if (currentUpgradeLevel == 1)
        {
            silverUpgrade.SetActive(true);
        }
        else if (currentUpgradeLevel == 2)
        {
            goldUpgrade.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!infoWindow.isActiveAndEnabled)
        {
            infoWindow.gameObject.SetActive(true);
        }
        VolumeManager.instance.GetComponent<AudioManager>().PlayButtonHoverSound();
        infoWindow.UpdateUpgradeInfoWindowUI(currentEffect[currentUpgradeLevel], 
            upgradeCosts[currentUpgradeLevel], upgradeEffect[currentUpgradeLevel], this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Scale(2f, 0.5f, () => { });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Scale(1, 0.2f, () => { });
    }

    private void Scale(float endValue, float duration, TweenCallback onEnd)
    {

        if (fadeTween!= null)
        {
            fadeTween.Kill(false);
        }

        fadeTween = image.transform.DOScale(endValue, duration);
        fadeTween.onComplete += onEnd;
    }

    public void Upgrade()
    {
        if (currentUpgradeLevel <= 1) //Sicherstellen, dass nach max Level der Count nicht höher geht
        {
            currentUpgradeLevel++;
            
            SetVisuellUpgrade();
            
            switch (upgradeType)
            {
                case UpgradeType.CANNON:
                    GameManager.instance.shipCannonLevel++;
                    break;
                case UpgradeType.CAPTAIN:
                    GameManager.instance.shipCaptainLevel++;
                    break;
                case UpgradeType.QUARTERS:
                    GameManager.instance.shipQuartersLevel++;
                    break;
                case UpgradeType.HULL:
                    GameManager.instance.shipHullLevel++;
                    break;
            }
            infoWindow.UpdateUpgradeInfoWindowUI(currentEffect[currentUpgradeLevel], 
                upgradeCosts[currentUpgradeLevel], upgradeEffect[currentUpgradeLevel], this);
        }
        
    }
}
public enum UpgradeType
{
    CANNON, CAPTAIN, QUARTERS, HULL
}
