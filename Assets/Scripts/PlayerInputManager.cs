using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    //Verantwortlich für das Verartbeiten und Weiterleiten der Tasteninputs des Spielers
    
    public bool allowInput = true;
    
    [Header("GameObjects")]
    public Canvas pauseMenuCanvas;
    public GameObject cheatMenu;
    
    //Variablen - Privat
    private PlayerInput input;
    private bool gameIsPaused;
    private bool cheatMenuOpen;

    //Events
    public static event Action onConfirmEvent;
    
    private void Awake()
    {
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Esc.performed += OnESCInput;
        input.Player.Confirm.performed += OnConfirmInput;
        input.Player.CheatMenu.performed += OnCheatInput;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Esc.performed -= OnESCInput;
        input.Player.Confirm.performed -= OnConfirmInput;
        input.Player.CheatMenu.performed -= OnCheatInput;
    }
    
    private void OnESCInput(InputAction.CallbackContext obj)
    {
        if (obj.performed & allowInput && SceneManager.GetActiveScene().ToString() != "Scene_MainMenu")
        {
            PauseMenu();
        }
    }
    
    public void PauseMenu()
    {
        if (!gameIsPaused)
        {
            pauseMenuCanvas.gameObject.SetActive(true);
            gameIsPaused = true;
        }
        else
        {
            pauseMenuCanvas.gameObject.SetActive(false);
            gameIsPaused = false;
        }
        
    }

    private void OnConfirmInput(InputAction.CallbackContext obj)
    {
        if (obj.performed & allowInput)
        {
            onConfirmEvent?.Invoke();
        }
    }
    
    private void OnCheatInput(InputAction.CallbackContext obj)
    {
        if (cheatMenu != null)
        {
            if (!cheatMenuOpen)
            {
                cheatMenu.SetActive(true);
                cheatMenuOpen = true;
            }
            else
            {
                cheatMenu.SetActive(false);
                cheatMenuOpen = false;
            }
        }
        else
        {
            Debug.LogWarning("CheatMenu nicht in der Szene!");
            VolumeManager.instance.GetComponent<AudioManager>().PlayPlatzHalterTeller();
        }
        
    }
}
