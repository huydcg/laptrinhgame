using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float lifeTime = 3f; // Đạn tự hủy sau 3 giây

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        GameObject hitObject = hitInfo.gameObject;

        // Xử lý va chạm với Tường
        if (hitObject.CompareTag("Wall"))
        {
            // Đạn của người chơi bị hủy khi chạm tường
            Destroy(gameObject);
            return; // Ngừng xử lý tiếp
        }

        // Xử lý va chạm với Player (Bỏ qua đạn chạm Player)
        if (hitObject.CompareTag("Player"))
        {
            // Đạn của Player, không làm gì khi chạm vào Player
            return;
        }

        // 🎯 Xử lý va chạm với Tank Địch (TANK AI) 🎯
        if (hitObject.CompareTag("Enemy"))
        {
            // 1. Tìm script EnemyAI trên Tank Địch
            EnemyAI enemyAI = hitObject.GetComponent<EnemyAI>();

            if (enemyAI != null)
            {
                // 2. Gọi hàm làm biến mất Tank Địch
                enemyAI.DestroyEnemyTank();
            }

            // 3. Hủy viên đạn (vì đã hoàn thành nhiệm vụ)
            Destroy(gameObject);
            return; // Ngừng xử lý tiếp
        }
    }
}