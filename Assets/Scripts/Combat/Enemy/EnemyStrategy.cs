using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrategy : MonoBehaviour
{
    //verantwortlich für Strategiefinmdung und Ausführung
    

    #region Strategiefindung

    //Strategiefindung

    #endregion

    #region Ausführung

    //Ausführuhng

    #endregion
    
    
}

public enum Strategies
{
    offensive, defensive, focus, massplay
}

//Offensive(Viele Karten im Spiel/Keine mehr auf der Hand): Alle möglichen Angriffe machen
//Defensive(Gegner hat Karten mit grossem Angriff): Mit Karten gegnerische Anfriffe abblocken 
//Focus(DangerLevel einer oder vieler Karten zu hoch): Karten mit dem höchsten DangerLevel töten
//Massplay(Mehr als 3 Karten auf der Hand): Alle möglichen Karten spielen

//Prio: DangerLevelFokus>Angriffe>Rückzüge>Karten spielen