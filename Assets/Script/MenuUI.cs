using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("game");
    }

    public void Credit()
    {

    }

    public void Settings()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
