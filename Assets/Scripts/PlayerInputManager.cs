using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    
    public bool allowInput = true;
    
    [Header("GameObjects")]
    public Canvas pauseMenuCanvas;
    
    //Variablen - Privat
    private PlayerInput input;
    private bool gameIsPaused;

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
    }

    

    private void OnDisable()
    {
        input.Disable();
        input.Player.Esc.performed -= OnESCInput;
        input.Player.Confirm.performed -= OnConfirmInput;
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
        if (!gameIsPaused && allowInput)
        {
            pauseMenuCanvas.gameObject.SetActive(true);
            gameIsPaused = true;
        }
        else if (allowInput)
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
}
