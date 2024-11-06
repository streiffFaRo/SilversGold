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
        FindObjectOfType<PlayerInputManager>().PauseMenu();
    }

    public void MainMenu()
    {
        VolumeManager.instance.GetComponent<AudioManager>().PlayMenuMusic();
        VolumeManager.instance.GetComponent<AudioManager>().StopGameMusic();
        SceneManager.LoadScene("Scene_MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }
    
}
