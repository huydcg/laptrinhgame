using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float xMin, xMax, yMin, yMax;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        xMin = bottomLeft.x + 0.5f;
        xMax = topRight.x - 0.5f;
        yMin = bottomLeft.y + 0.5f;
        yMax = topRight.y - 0.5f;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;

        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, xMin, xMax);
        clampedPos.y = Mathf.Clamp(clampedPos.y, yMin, yMax);
        transform.position = clampedPos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Star"))
        {
            GameManager.instance.AddScore(10); // +10 điểm
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Meteor"))
        {
            // Thua game
            PlayerPrefs.SetInt("Score", GameManager.instance.score);
            SceneManager.LoadScene("EndGame");
        }
    }

}
