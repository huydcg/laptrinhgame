using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int totalEnemies;
    private float timeCounter = 0f;
    private bool gameEnded = false;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        // Đếm số enemy ban đầu
        totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        UnityEngine.Debug.Log("Enemy ban đầu: " + totalEnemies);
    }

    void Update()
    {
        if (!gameEnded)
            timeCounter += Time.deltaTime;
    }

    //------------------------
    // NGƯỜI CHƠI THUA
    //------------------------
    public void PlayerLose()
    {
        if (gameEnded) return;
        gameEnded = true;

        SaveResultToPlayerPrefs("lose");
        SceneManager.LoadScene("EndGame");
    }

    //------------------------
    // MỖI KHI ENEMY CHẾT
    //------------------------
    public void EnemyDied()
    {
        if (gameEnded) return;

        totalEnemies--;
        UnityEngine.Debug.Log("Enemy còn lại: " + totalEnemies);

        if (totalEnemies <= 0)
            PlayerWin();
    }

    //------------------------
    // NGƯỜI CHƠI THẮNG
    //------------------------
    void PlayerWin()
    {
        if (gameEnded) return;
        gameEnded = true;

        SaveResultToPlayerPrefs("win");
        SceneManager.LoadScene("EndGame");
    }

    //------------------------
    // LƯU DỮ LIỆU
    //------------------------
    void SaveResultToPlayerPrefs(string state)
    {
        PlayerPrefs.SetString("GameState", state);

        int minutes = Mathf.FloorToInt(timeCounter / 60);
        int seconds = Mathf.FloorToInt(timeCounter % 60);
        string finalTime = minutes.ToString("00") + ":" + seconds.ToString("00");

        PlayerPrefs.SetString("FinalTime", finalTime);

        PlayerPrefs.Save();
    }
}
