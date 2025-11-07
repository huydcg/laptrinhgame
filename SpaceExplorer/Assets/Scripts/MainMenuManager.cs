using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class MainMenuManager : MonoBehaviour
{
    public GameObject instructionsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
    }

    public void CloseInstructions()
    {
        instructionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("Quit Game!");
        UnityEngine.Application.Quit();
    }
}
