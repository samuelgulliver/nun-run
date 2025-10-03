using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement; // allows us to manage the scene.

public class MainMenu : MonoBehaviour
{

    // you have event listener and event handler
    public void PlayGame()
    {
        print("egg");
        SceneManager.LoadScene("Level 1");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game Requested");

        #if UNITY_EDITOR
                // If running in the editor
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    // If running in a build
                    Application.Quit();
        #endif
    }
}
