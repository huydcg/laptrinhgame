using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // đổi "GameScene" thành tên scene game của bạn
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }
}
