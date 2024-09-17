using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WaitForSeconds = UnityEngine.WaitForSeconds;

public class BattleSystem : MonoBehaviour
{
    //Verantwortlich für Schlachtverlauf, Bestimmt momentaner Spielstatus
    
    public BattleState state;
    
    [Header("Scripts")]
    public DeckManager deckManager;
    public PlayerManager playerManager;
    public EnemyManager enemyManager;

    [Header("Other")]
    public GameObject blurImage;
    
    private void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());
    }

    private IEnumerator SetUpBattle()
    {
        //TODO Level Number Banner
        //TODO Enemy Ship rolling in
        //TODO Enemy Dialogue
        yield return new WaitForSeconds(1f);
        StartCoroutine(DrawStartCards());
        yield return new WaitForSeconds(3f);
        PlayerTurn();
    }

    private IEnumerator DrawStartCards()
    {
        deckManager.DrawCards();
        enemyManager.DrawCards();
        //TODO Enemy Card draw
        yield return new WaitForSeconds(1f);
        deckManager.DrawCards();
        enemyManager.DrawCards();
        //TODO Enemy Card draw
        yield return new WaitForSeconds(1f);
        deckManager.DrawCards();
        enemyManager.DrawCards();
        //TODO Enemy Card draw
    }

    public void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        playerManager.StartNewTurn();
    }

    public void EnemyTurn()
    {
        state = BattleState.ENEMYTURN;
        enemyManager.StartNewEnemyTurn();
    }

    public void PlayerWon()
    {
        StartCoroutine(Win());
    }

    private IEnumerator Win()
    {
        state = BattleState.WON;
        Debug.Log("Won");
        yield return new WaitForSeconds(1f);
        blurImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        //TODO Neue Karte Rekrutieren
        GameManager.instance.UpdateLevel();
        yield return new WaitForSeconds(2f);
        ReloadScene();
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

    public void ReloadScene()
    {
        SceneManager.LoadScene("Scene_MainMenu");
    }
}

public enum BattleState
{
    START, PLAYERTURN, ENEMYTURN, WON, LOST
}
