using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class events : MonoBehaviour
{
    public void ReplayGame() 
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
