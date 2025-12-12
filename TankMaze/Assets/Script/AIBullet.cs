using UnityEngine;
using System.Diagnostics;

public class AIBullet : MonoBehaviour
{
    public float speed = 15f;           // Tốc độ đạn bay
    public float maxLifeTime = 5f;      // Thời gian tồn tại tối đa

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, maxLifeTime);
    }

    // Xử lý khi đạn AI (đang là Trigger) chạm vào một Collider khác
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Kiểm tra xem đạn có chạm vào Player không
        if (other.CompareTag("Player"))
        {
            // 2. Gọi hàm làm biến mất Player
            TankPlayer tankPlayer = other.GetComponent<TankPlayer>();

            if (tankPlayer != null)
            {
                // Hàm này sẽ được viết trong script PlayerTank (Bước 3)
                tankPlayer.DestroyPlayer();
            }

            // 3. Hủy viên đạn (Sau khi đã gây sát thương)
            Destroy(gameObject);
        }

        // Lưu ý: Đạn vẫn xuyên Tường vì đã tắt Layer interaction Matrix.
    }
}