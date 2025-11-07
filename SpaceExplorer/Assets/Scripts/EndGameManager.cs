using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

public class EndGameManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        int score = PlayerPrefs.GetInt("Score", 0);
        finalScoreText.text = "Your Score: " + score.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
