using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarController : MonoBehaviour
{
    public float speed = 6f;

    void Update()
    {
        // Sao di chuyển sang phải
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Kiểm tra nếu vượt khỏi màn hình bên phải
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x > 1.05f)
        {
            // Reset vị trí sao
            transform.position = new Vector3(-9f, UnityEngine.Random.Range(-4f, 4f), 0);

            // Tăng biến chung trong GameManager
            GameManager.instance.starsMissed++;

            // Kiểm tra nếu vượt quá giới hạn
            if (GameManager.instance.starsMissed >= GameManager.instance.maxMissed)
            {
                PlayerPrefs.SetInt("Score", GameManager.instance.score);
                SceneManager.LoadScene("EndGame");
            }
        }
    }
}
