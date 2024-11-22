using UnityEngine;

public class CheatMenu : MonoBehaviour
{
    //Verantwortlich für einfaches Testen jeglicher Funktionen des Spiels
    
    //TODO Cheatmode ausmachen für finalen Build!
    
    //Private Scripts
    private PlayerManager playerManager;
    private BattleSystem battleSystem;
    private GameManager gameManager;
    private AudioManager audioManager;
    
    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        battleSystem = FindObjectOfType<BattleSystem>();
        gameManager = GameManager.instance;
        audioManager = VolumeManager.instance.GetComponent<AudioManager>();
    }

    public void CheatBooty()
    {
        gameManager.booty += 100;
        audioManager.PlayBootySound();
        FindObjectOfType<BootyUI>()?.UpdateBootyUI();
    }

    public void FullHeal()
    {
        if (playerManager != null)
        {
            audioManager.PlayUpgradeSound();
            playerManager.UpdateHealth(100, true);
        }
        else
        {
            Debug.LogWarning("Nicht in CombatScene!");
        }
    }

    public void FullCommandPower()
    {
        if (playerManager != null)
        {
            audioManager.PlayPlatzHalterFlasche();
            playerManager.currentCommandPower = playerManager.maxCommandPower;
            playerManager.commandPowerText.text = playerManager.currentCommandPower.ToString();
            
        }
        else
        {
            Debug.LogWarning("Nicht in CombatScene!");
        }
    }

    public void WinGame()
    {
        if (battleSystem != null)
        {
            battleSystem.PlayerWon();
        }
        else
        {
            Debug.LogWarning("Nicht in CombatScene!");
        }
    }

    public void LooseGame()
    {
        if (battleSystem != null)
        {
            battleSystem.GameOver();
        }
        else
        {
            Debug.LogWarning("Nicht in CombatScene!");
        }
    }
}
