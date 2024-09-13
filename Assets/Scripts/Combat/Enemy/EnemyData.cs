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
    public Strategies strategies;
    public List<Card> deckToPrepare = new List<Card>();
}


