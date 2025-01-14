using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Method for starting the game
    public void StartGame()
    {
        Debug.Log("Start button clicked!");
        SceneManager.LoadScene("Zion"); // Replace with your scene name
    }

    // Method for quitting the game
    public void QuitGame()
    {
        Debug.Log("Quit button clicked!");
        Application.Quit();
    }
}
