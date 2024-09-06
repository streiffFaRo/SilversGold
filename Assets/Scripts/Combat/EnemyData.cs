using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public string enemyTitle;
    public int level;
    public int health;
    public int commandPower;
    public Strategies strategies;
}

public enum Strategies
{
    offensive, defensive, focus, massplay
}
