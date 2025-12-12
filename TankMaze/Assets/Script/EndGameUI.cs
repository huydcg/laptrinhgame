using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

public class EndGameUI : MonoBehaviour
{
    public GameObject winText;
    public GameObject loseText;
    public TMP_Text timeText;

    void Start()
    {
        string state = PlayerPrefs.GetString("GameState");
        string finalTime = PlayerPrefs.GetString("FinalTime");

        timeText.text = "Time: " + finalTime;

        if (state == "win")
        {
            winText.SetActive(true);
        }
        else
        {
            loseText.SetActive(true);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
