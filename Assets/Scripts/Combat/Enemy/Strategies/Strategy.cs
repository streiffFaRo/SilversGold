using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Strategy", menuName = "Strategy")]
public class Strategy : ScriptableObject
{
    public new string name;
    public float drawMod =1;
    public float playMod =1;
    public float emptyLaneMod =1;
    public float attackMod =1;
    public float retreatMod =1;
    public float broadsideMod =1;
}