using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Image = UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    [Header("CoreInfos")]
    public new string name;
    public string description;
    public string position;
    public Sprite artwork;
    
    [Header("Stats")]
    public int cost;
    public int attack;
    public int defense;
    
    [Header("MetaInfos")]
    public int tier;
    public int dangerLevel;
    public int pointsFielded;
    public int pointsAttack;
    public int pointsRetreat;

    [Header("Keywords")]
    public bool keyWordCannoneer;
    public bool keyWordFielded;
    public bool keyWordDeath;
    public bool keyWordRetreat;

    [Header("CardEffect")]
    public MonoScript script;
    public int para1;
    public int para2;
    public float para3;
    public bool para4;
    public Card para5;
}
