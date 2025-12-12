using System.Diagnostics;
using UnityEngine;

public class TankPlayer : MonoBehaviour
{
    // Khai báo biến tốc độ và Rigidbody
    public float speed = 0.5f;        // Tốc độ di chuyển
    public float rotationSpeed = 10f; // Tốc độ xoay (độ/giây)
    private Rigidbody2D rb;

    public GameObject bulletPrefab;     // Prefab đạn (sẽ gán trong Inspector)
    public Transform firePoint;         // Điểm phát đạn (sẽ gán trong Inspector)
    public float bulletSpeed = 4f;     // Tốc độ đạn
    public float fireRate = 0.5f;       // Tốc độ bắn (Thời gian chờ giữa 2 viên)
    private float nextFireTime = 0f;    // Bộ đếm thời gian chờ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Lấy Input Di chuyển (W/S hoặc Up/Down)
        float moveInput = Input.GetAxis("Vertical");

        // 2. Lấy Input Xoay (A/D hoặc Left/Right)
        float rotationInput = Input.GetAxis("Horizontal");

        // 3. Xoay Tank (Sử dụng trái/phải để xoay)
        float rotationAmount = -rotationInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, rotationAmount);

        // 4. Di chuyển và Dừng Tank (Đã sửa)
        if (moveInput != 0) // Nếu người chơi nhấn W hoặc S
        {
            // SỬA ĐỔI QUAN TRỌNG: Dùng transform.right thay vì transform.up
            // để tank di chuyển theo hướng mà nó đang nhìn (trục X cục bộ).
            Vector2 forwardDirection = transform.right;

            // moveInput > 0 (W/Up) sẽ di chuyển về phía trước, moveInput < 0 (S/Down) sẽ lùi lại.
            Vector2 movement = forwardDirection * moveInput * speed;

            rb.linearVelocity = movement;
        }
        else // Nếu người chơi không nhấn phím di chuyển
        {
            // Buộc tank dừng lại ngay lập tức
            rb.linearVelocity = Vector2.zero;
        }

        // >> LOGIC BẮN <<
        // Kiểm tra Input Bắn (mặc định là Chuột Trái) và kiểm tra Cooldown
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            // Thiết lập thời gian chờ cho lần bắn tiếp theo
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // 1. Tạo đạn tại vị trí và góc xoay của FirePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 2. Phóng đạn
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();

        // Lấy hướng tiến của FirePoint (transform.right) và phóng đạn theo hướng đó
        // Vì FirePoint là con của Tank, hướng của nó chính là hướng thân Tank
        rbBullet.linearVelocity = firePoint.right * bulletSpeed;
    }

    public void DestroyPlayer()
    {
        UnityEngine.Debug.Log("Player bị phá hủy bởi đạn AI!");

        // Hủy player
        Destroy(gameObject);

        // Gọi GameManager một cách an toàn
        if (GameManager.instance != null)
        {
            GameManager.instance.PlayerLose();
        }
        else
        {
            UnityEngine.Debug.LogError("❌ GameManager.instance == NULL — không tìm thấy GameManager trong Scene!");
        }
    }


}