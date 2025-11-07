using UnityEngine;

public class MeteorController : MonoBehaviour
{
    public float speed = 8f;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < -10f)
            Destroy(gameObject);
    }
}
