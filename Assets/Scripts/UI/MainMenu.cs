using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    
    
    public void PlayGame()
    {
        SceneManager.LoadScene("Scene_Content");
    }

    public void Settings()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }
    
}
