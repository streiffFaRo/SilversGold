using UnityEngine;


[CreateAssetMenu(fileName = "New Strategy", menuName = "Strategy")]
public class Strategy : ScriptableObject
{
    public new string name;
    public float drawMod =1; //Verlangen Karten zu ziehen
    public float playMod =1; //Verlangen Karten zu spielen
    public float emptyLaneMod =1; //Verlangen Karten auf offene Linien zu platzieren
    public float attackMod =1; //Verlangen mit Karten anzugreiffen
    public float retreatMod =1; //Verlangen Karten zurück zu ziehen
    public float broadsideMod =1; //Verlangen Breitseiten zu schiessen
}