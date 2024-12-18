using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject levelInfo;
    public GameObject playerTurnInfo;
    public GameObject enemyTurnInfo;
    public GameObject fogEffect;
    
    //Events
    public static event Action onPlayerTurnEvent;
    public static event Action onEnemyTurnEvent;
    
    private void Start()
    {
        state = BattleState.START;
        blurImage.SetActive(true);
        StartCoroutine(SetUpBattle());

        if (GameManager.instance.currentLevel == 11)
        {
            //TODO fogEffect.SetActive(true);
        }
    }

    private IEnumerator SetUpBattle()
    {
        yield return new WaitForSeconds(0.5f);
        VolumeManager.instance.GetComponent<AudioManager>().PLayEndTurnBellSound();
        //Levelbanner Einblendung
        levelInfo.SetActive(true);
        levelInfo.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + GameManager.instance.currentLevel;
        yield return new WaitForSeconds(1f);
        StartCoroutine(DrawStartCards()); //Startkaten werden gezogen
        yield return new WaitForSeconds(1f);
        levelInfo.SetActive(false);
        blurImage.SetActive(false);
        yield return new WaitForSeconds(2f);
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
        VolumeManager.instance.GetComponent<AudioManager>().PLayEndTurnBellSound();
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
        state = BattleState.ENEMYTURN;
        VolumeManager.instance.GetComponent<AudioManager>().PLayEndTurnBellSound();
        enemyTurnInfo.SetActive(true);
        deckManager.SetAllOtherButtonsPassive();
        yield return new WaitForSeconds(1f);
        enemyTurnInfo.SetActive(false);
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
        if (GameManager.instance.currentLevel >= 11)
        {
            SceneManager.LoadScene("Scene_Ship");
        }
        else
        {
            recruitManager.ShowRecruitmentOptions();
        }
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
