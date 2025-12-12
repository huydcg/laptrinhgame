using System.Diagnostics;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Cài đặt chung
    public float moveSpeed = 0.5f;
    public float rotationSpeed = 100f; // Tăng tốc độ xoay lên 100f để xoay nhanh hơn
    public float patrolTime = 5f;      // Thời gian đổi hướng khi không đụng tường
    public float detectionRange = 7f;
    public float stoppingDistance = 4f;
    public GameObject aiBulletPrefab; // Prefab đạn AI
    public Transform firePoint;       // Điểm nòng súng để đạn xuất phát
    public float fireRate = 3f;       // Tốc độ bắn (1 viên / 3 giây)
    private float nextFireTime;       // Thời điểm bắn tiếp theo

    // ... (Các hàm Start(), Update(), PatrolState() giữ nguyên) ...

    // Cài đặt lùi và xoay
    public float reverseDuration = 1f;   // Thời gian lùi
    public float reverseSpeed = 0.3f;    // Tốc độ lùi (thấp hơn moveSpeed)

    // Biến quản lý trạng thái tuần tra
    private enum PatrolStateEnum { Moving, Reversing, Rotating }
    private PatrolStateEnum currentPatrolState = PatrolStateEnum.Moving;

    private Transform playerTarget;
    private Rigidbody2D rb;

    private float nextDirectionChangeTime;
    private Vector2 targetRotationDirection; // Hướng mục tiêu khi xoay 90 độ
    // BIẾN 'shouldRotate' ĐÃ ĐƯỢC XÓA ĐỂ LOẠI BỎ CẢNH BÁO CS0414

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }

        nextDirectionChangeTime = Time.time + patrolTime;
    }

    void Update()
    {
        if (playerTarget == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer <= detectionRange)
        {
            // [TRẠNG THÁI 1: TẤN CÔNG / BÁM THEO]
            AttackState(distanceToPlayer);
        }
        else
        {
            // [TRẠNG THÁI 2: TUẦN TRA]
            PatrolState();
        }
    }

    void AttackState(float distanceToPlayer)
    {
        // 1. Dừng hoàn toàn (vì phát hiện Player)
        rb.linearVelocity = Vector2.zero;

        // Đảm bảo trạng thái tuần tra được reset
        if (currentPatrolState != PatrolStateEnum.Moving)
        {
            currentPatrolState = PatrolStateEnum.Moving;
        }

        // 2. Xoay để đối mặt với Người chơi
        RotateTowards(playerTarget.position);

        // 3. Bắn đạn nếu đã đến lúc
        if (Time.time >= nextFireTime)
        {
            // Chỉ bắn khi đã xoay gần đúng hướng Player (Giảm tần suất bắn nếu Tank đang xoay)
            if (IsAimedAtPlayer())
            {
                FireBullet();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void FireBullet()
    {
        if (aiBulletPrefab != null && firePoint != null)
        {
            // Tạo viên đạn tại FirePoint và xoay theo hướng của FirePoint
            Instantiate(aiBulletPrefab, firePoint.position, firePoint.rotation);

            // Có thể thêm hiệu ứng âm thanh/hình ảnh khi bắn 
        }
    }

    bool IsAimedAtPlayer()
    {
        Vector2 directionToPlayer = (playerTarget.position - transform.position).normalized;
        // Góc giữa hướng tank (transform.right) và hướng Player
        float angle = Vector2.Angle(transform.right, directionToPlayer);

        // Nếu góc nhỏ hơn 5 độ, coi như đã nhắm bắn thành công
        return angle < 5f;
    }

    bool IsFacingWall()
    {
        float distanceToCheck = 0.6f;
        int layerMask = LayerMask.GetMask("Wall");

        UnityEngine.Debug.DrawRay(transform.position, transform.right * distanceToCheck, Color.red, 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, distanceToCheck, layerMask);

        return hit.collider != null;
    }

    // Logic Tuần tra chính
    void PatrolState()
    {
        // Xử lý đổi hướng nếu hết giờ và đang di chuyển
        if (Time.time >= nextDirectionChangeTime && currentPatrolState == PatrolStateEnum.Moving)
        {
            // Tạm dừng di chuyển và chuyển sang xoay ngẫu nhiên
            rb.linearVelocity = Vector2.zero;
            PrepareRotation();
        }

        switch (currentPatrolState)
        {
            case PatrolStateEnum.Moving:
                // Nếu đụng tường, bắt đầu lùi lại
                if (IsFacingWall())
                {
                    currentPatrolState = PatrolStateEnum.Reversing;
                    // Bắt đầu lùi và hẹn giờ chuyển trạng thái
                    rb.linearVelocity = -transform.right * reverseSpeed;
                    Invoke("FinishReversing", reverseDuration);
                }
                else
                {
                    // Di chuyển thẳng
                    rb.linearVelocity = transform.right * moveSpeed;
                }
                break;

            case PatrolStateEnum.Reversing:
                // Tank chỉ lùi lại (vận tốc đã được thiết lập ở trên)
                // Đang chờ Invoke("FinishReversing", ...)
                break;

            case PatrolStateEnum.Rotating:
                // Xoay mượt mà đến hướng mục tiêu
                RotateTowardsDirection(targetRotationDirection);
                rb.linearVelocity = Vector2.zero; // Dừng hoàn toàn khi đang xoay

                // Kiểm tra xem đã xoay xong chưa
                if (CheckRotationComplete())
                {
                    // Xoay xong, chuyển sang di chuyển
                    currentPatrolState = PatrolStateEnum.Moving;
                    nextDirectionChangeTime = Time.time + patrolTime;
                }
                break;
        }
    }

    // Hàm được gọi sau khi lùi xong
    void FinishReversing()
    {
        // Dừng lùi và chuẩn bị xoay 90 độ
        rb.linearVelocity = Vector2.zero;
        PrepareRotation(true); // Tham số true báo hiệu phải xoay 90 độ
    }

    // Chuẩn bị hướng xoay mới
    void PrepareRotation(bool turn90Degrees = false)
    {
        // Nếu không đụng tường (hết giờ), chọn hướng 1 trong 4 ngẫu nhiên
        if (!turn90Degrees)
        {
            int rand = UnityEngine.Random.Range(0, 4);
            if (rand == 0) targetRotationDirection = Vector2.up;
            else if (rand == 1) targetRotationDirection = Vector2.down;
            else if (rand == 2) targetRotationDirection = Vector2.left;
            else targetRotationDirection = Vector2.right;
        }
        else // Nếu đụng tường, chọn hướng xoay 90 độ trái hoặc phải
        {
            // Tối ưu hóa logic xoay 90 độ dựa trên hướng hiện tại
            Vector2 currentDirection = transform.right;
            float turnAngle = (UnityEngine.Random.Range(0, 2) == 0) ? 90f : -90f; // 90 hoặc -90 độ
            Quaternion rotation = Quaternion.Euler(0, 0, turnAngle);
            targetRotationDirection = rotation * currentDirection;

            // Logic cũ (vẫn hoạt động):
            // float currentAngle = transform.eulerAngles.z;
            // float turnAngle = (UnityEngine.Random.Range(0, 2) == 0) ? 90f : -90f; 
            // float newAngle = currentAngle + turnAngle;
            // targetRotationDirection = Quaternion.Euler(0, 0, newAngle) * Vector2.right;
        }

        currentPatrolState = PatrolStateEnum.Rotating;
    }

    // Kiểm tra xem góc đã gần đạt mục tiêu chưa (dùng cho trạng thái Rotating)
    bool CheckRotationComplete()
    {
        // Tính góc mục tiêu
        float targetAngle = Mathf.Atan2(targetRotationDirection.y, targetRotationDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        // So sánh góc hiện tại và góc mục tiêu (epsilon là một giá trị nhỏ để chấp nhận sai số)
        return Quaternion.Angle(transform.rotation, targetRotation) < 1f;
    }

    // **********************************
    // HÀM XOAY VÀ DI CHUYỂN
    // **********************************

    void MoveTank(float currentDistance)
    {
        if (currentDistance > stoppingDistance)
        {
            // Sử dụng rb.velocity hoặc rb.linearVelocity. Giữ nguyên rb.linearVelocity
            rb.linearVelocity = transform.right * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void RotateTowardsDirection(Vector2 direction)
    {
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }

    public void DestroyEnemyTank()
    {
        UnityEngine.Debug.Log(gameObject.name + " đã bị tiêu diệt bởi Player!");

        // Tùy chọn: Thêm hiệu ứng vụ nổ trước khi hủy đối tượng
        // Instantiate(explosionPrefab, transform.position, Quaternion.identity); 

        // Hủy đối tượng Tank Địch này
        Destroy(gameObject);
        GameManager.instance.EnemyDied();
    }

}