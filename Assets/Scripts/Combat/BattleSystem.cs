using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    
    public BattleState state;
    public DeckManager deckManager;
    public EnemyManager enemyManager;
    
    
    private void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());
    }

    public IEnumerator SetUpBattle()
    {
        Debug.Log("Battle Starts");
        //TODO Level Number Banner
        //TODO Enemy Ship rolling in
        //TODO Enemy Dialogue
        yield return new WaitForSeconds(2f);
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        deckManager.StartNewTurn();
        Debug.Log("Player Turn Starts");
    }

    public void EnemyTurn()
    {
        state = BattleState.ENEMYTURN;
        enemyManager.StartNewEnemyTurn();
        Debug.Log("EnemyTurn");
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

    public void GameOver()
    {
        StartCoroutine(Lost());
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
