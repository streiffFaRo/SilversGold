using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipUpgradeArea : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Verantwortlich die Infos über den momentanen Stand der Upgrades, Autausch der Schiffsteile bei upgrades

    public UpgradeType upgradeType;
    public UpgradeInfoWindowUI infoWindow;
    
    [Header("UpgradeInformation")]
    public int currentUpgradeLevel;
    
    public string[] currentObjectName;
    public string[] currentEffect;
    public int[] upgradeCosts;
    public string[] upgradedObjectName;
    public string[] upgradeEffect;
    
    public Image image;

    private Tween fadeTween;

    private void Start()
    {
        var color = image.color;
        color.a = 0;
        image.color = color;
        
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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        infoWindow.UpdateUpgradeInfoWindowUI(currentObjectName[currentUpgradeLevel], currentEffect[currentUpgradeLevel], 
            upgradeCosts[currentUpgradeLevel], upgradedObjectName[currentUpgradeLevel], upgradeEffect[currentUpgradeLevel], this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Fade(0.6f, 0.5f, () => { });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Fade(0, 0.2f, () => { });
    }

    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {

        if (fadeTween!= null)
        {
            fadeTween.Kill(false);
        }

        fadeTween = image.DOFade(endValue, duration);
        fadeTween.onComplete += onEnd;
    }

    public void Upgrade()
    {
        currentUpgradeLevel++;
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
        //TODO Imageaustausch (Visuell) bei Upgrade -> Bsp. Kanonen von Schwarz zu Silber zu Gold ...
        infoWindow.UpdateUpgradeInfoWindowUI(currentObjectName[currentUpgradeLevel], currentEffect[currentUpgradeLevel], 
            upgradeCosts[currentUpgradeLevel], upgradedObjectName[currentUpgradeLevel], upgradeEffect[currentUpgradeLevel], this);
    }
}
public enum UpgradeType
{
    CANNON, CAPTAIN, QUARTERS, HULL
}
