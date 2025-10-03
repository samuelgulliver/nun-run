using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLogic : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject endGameScreen;
    public void Level2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void Level3()
    {
            SceneManager.LoadScene("Level 3");
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void EndGame()
    {
        Debug.Log("End Game method called");
        endGameScreen.SetActive(true);
    }

    public void GameOver()
    {
        Debug.Log("Game over method called");
        gameOverScreen.SetActive(true);
    }
}
