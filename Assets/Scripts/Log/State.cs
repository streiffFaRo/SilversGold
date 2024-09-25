using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class State
{
    [Tooltip("Status ID oder Codename")]
    public string id;
    
    [Tooltip("Anzahl des Status")]
    public int amount;


    public State(string id, int amount)
    {
        this.id = id;
        this.amount = amount;

    }
}
