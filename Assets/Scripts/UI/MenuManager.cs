using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //Verantwortlich für die Funktionalität des Menüs
    
    public void PlayGame()
    {
        SceneManager.LoadScene("Scene_Log");
        GameManager.instance.SetUpNewGame();
    }

    public void Continue()
    {
        FindObjectOfType<PlayerInputManager>().PauseMenu();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Scene_MainMenu");
    }

    public void Credits()
    {
        
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }
    
}
