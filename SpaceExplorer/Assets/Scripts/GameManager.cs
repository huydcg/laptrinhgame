using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score = 0;
    private bool isGameOver = false;
    public static GameManager instance;
    public int starsMissed = 0; // số sao bay ra ngoài
    public int maxMissed = 3;   // giới hạn thua


    void Start()
    {
        scoreText.text = "Score: 0";
    }

    public void AddScore(int value)
    {
        if (isGameOver) return;
        score += value;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        isGameOver = true;
        PlayerPrefs.SetInt("LastScore", score);
        SceneManager.LoadScene("EndGame");
    }

    void Awake()
    {
        instance = this;
    }
}
