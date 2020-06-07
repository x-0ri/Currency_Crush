using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitcher : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Player has quit the game");
        Application.Quit();
    }

    public void SaveAndExit()
    {
        SaveGame.Save();
        SceneManager.LoadScene("PlayLoadMenu");
        Debug.Log("Player used Save & Exit ");
    }

    public void NewGame()
    {
        Debug.Log("Player started new game ");
        SceneManager.LoadScene("Game");
    }

    public void ContinueGame()
    {
        GameBoard._LOADEDGAME = true;
        Debug.Log("Player continued game");
        SceneManager.LoadScene("Game");
    }
 }
