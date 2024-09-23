using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public string enemyTitle;
    public int health;
    public int commandPower;
    public int cannonLevel;
    public Strategy strategy;
    public List<Card> deckToPrepare = new List<Card>();
}


