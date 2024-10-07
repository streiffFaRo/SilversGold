using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Serialization;
using Image = UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;
    public string position;
    
    public Sprite artwork;
    
    public int cost;
    public int attack;
    public int defense;
    
    public int tier;
    public int dangerLevel;
    public int pointsFielded;
    public int pointsAttack;
    public int pointsRetreat;

    public bool keyWordCannoneer;
    public bool keyWordFielded;
    public bool keyWordDeath;
    public bool keyWordRetreat;
}
