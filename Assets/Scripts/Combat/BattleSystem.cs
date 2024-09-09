using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    
    public BattleState state;
    
    
    
    private void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());
    }

    private IEnumerator SetUpBattle()
    {
        Debug.Log("Battle Starts");
        //TODO Level Number Banner
        //TODO Enemy Ship rolling in
        //TODO Enemy Dialogue
        yield return new WaitForSeconds(2f);
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        Debug.Log("Player Turn Starts");
    }

    private void EnemyTurn()
    {
        state = BattleState.ENEMYTURN;
        Debug.Log("EnemyTurn");
        //TODO Enemy Logik auf EnemyManager ausführen
    }

    private IEnumerator Win()
    {
        state = BattleState.WON;
        Debug.Log("Won");
        //TODO Enemy Ship sinks
        yield return new WaitForSeconds(2f);
        //TODO Beute vergeben
        yield return new WaitForSeconds(2f);
        //TODO Neue Karte Rekrutieren
        yield return new WaitForSeconds(2f);
        //TODO Nächstes Level Laden
    }

    private IEnumerator Lost()
    {
        state = BattleState.LOST;
        Debug.Log("Lost");
        //TODO Player Ship sinks
        //TODO "Game Over" Einblendung
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Scene_MainMenu");
    }
}

public enum BattleState
{
    START, PLAYERTURN, ENEMYTURN, WON, LOST
}
