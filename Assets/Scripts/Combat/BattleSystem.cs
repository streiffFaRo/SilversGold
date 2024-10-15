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
    public RecruitManager recruitManager;

    [Header("Other")]
    public GameObject blurImage;
    public GameObject gameOverMenu;
    public GameObject playerTurnInfo;
    public GameObject enemyTurnInfo;
    
    //Events
    public static event Action onPlayerTurnEvent;
    public static event Action onEnemyTurnEvent;
    
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
        StartCoroutine(PlayerTurn());
    }

    private IEnumerator DrawStartCards()
    {
        deckManager.DrawCards();
        enemyManager.DrawCards();
        
        yield return new WaitForSeconds(1f);
        deckManager.DrawCards();
        enemyManager.DrawCards();
        
        yield return new WaitForSeconds(1f);
        deckManager.DrawCards();
        enemyManager.DrawCards();
        
    }

    public IEnumerator PlayerTurn()
    {
        VolumeManager.instance.GetComponent<AudioManager>().PlayPlatzHalterFlasche();
        playerTurnInfo.SetActive(true);
        yield return new WaitForSeconds(1f);
        playerTurnInfo.SetActive(false);
        state = BattleState.PLAYERTURN;
        playerManager.StartNewTurn();
        if (onPlayerTurnEvent != null)
        {
            onPlayerTurnEvent.Invoke();
        }
    }

    public IEnumerator EnemyTurn()
    {
        VolumeManager.instance.GetComponent<AudioManager>().PlayPlatzHalterFlasche();
        enemyTurnInfo.SetActive(true);
        yield return new WaitForSeconds(1f);
        enemyTurnInfo.SetActive(false);
        state = BattleState.ENEMYTURN;
        enemyManager.StartNewEnemyTurn();
        if (onEnemyTurnEvent != null)
        {
            onEnemyTurnEvent.Invoke();
        }
    }

    public void PlayerWon()
    {
        StartCoroutine(Win());
    }

    private IEnumerator Win()
    {
        state = BattleState.WON;
        Debug.Log("Won");
        VolumeManager.instance.GetComponent<AudioManager>().PlayShipDeathSound();
        yield return new WaitForSeconds(1f);
        blurImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        recruitManager.ShowBootyReward();
        VolumeManager.instance.GetComponent<AudioManager>().PlayBootySound();
        yield return new WaitForSeconds(3f);
        recruitManager.HideBootyReward();
        yield return new WaitForSeconds(1f);
        recruitManager.ShowRecruitmentOptions();
    }

    public void GameOver()
    {
        StartCoroutine(Lost());
    }
    
    private IEnumerator Lost()
    {
        state = BattleState.LOST;
        Debug.Log("Lost");
        VolumeManager.instance.GetComponent<AudioManager>().PlayShipDeathSound();
        yield return new WaitForSeconds(2f);
        blurImage.SetActive(true);
        gameOverMenu.SetActive(true);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Scene_Content");
    }
}

public enum BattleState
{
    START, PLAYERTURN, ENEMYTURN, WON, LOST
}
