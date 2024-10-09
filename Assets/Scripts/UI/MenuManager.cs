using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Scene_Log");
        GameManager.instance.SetUpNewGame();
    }

    public void Continue()
    {
        GameManager.instance.GetComponent<PlayerInputManager>().PauseMenu();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Scene_MainMenu");
        GameManager.instance.SetUpNewGame();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }
    
}
