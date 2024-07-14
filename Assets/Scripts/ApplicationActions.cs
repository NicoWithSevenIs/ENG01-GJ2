using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationActions 
{
    public static void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public static void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //for consistency
    public static void Quit()
    {
        Application.Quit();
    }

    
}
