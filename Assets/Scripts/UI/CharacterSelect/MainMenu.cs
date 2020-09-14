using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void DevShellQuickstart()
    {
        SceneManager.LoadScene("Prison_Passage_Dev");
    }

    public void QuitGame()
    {
        Debug.Log("You have selected Quit. In a real build of the game, this will shut down the program");
        Application.Quit();
    }
}
