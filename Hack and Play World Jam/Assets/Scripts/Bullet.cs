using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed = 15f;
    Rigidbody2D rb;
    public int bulletDurability;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Start()
    {
        // Initialize velocity from rotation
        rb.linearVelocity = transform.up * moveSpeed;
        Debug.Log($"Bullet durability is {bulletDurability}");
    }

    void FixedUpdate()
    {
        // Every physics step, velocity follows current rotation
        rb.linearVelocity = transform.up * moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy targets without bouncing
        if (collision.gameObject.CompareTag("Target"))
        {
            Destroy(collision.gameObject);
            return;
        }

        GameManager.Instance.GetTargetCount();

        // Get first contact normal
        if (collision.contactCount > 0)
        {
            Vector2 normal = collision.GetContact(0).normal;

            // Reflect the current forward vector (transform.up) around that normal
            Vector2 newDir = Vector2.Reflect(transform.up, normal).normalized;

            // Apply it by rotating the transform—this automatically changes velocity next FixedUpdate
            float newAngle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, newAngle);

            bulletDurability--;
            UpdateBounceText();
            if (bulletDurability <= 0)
            {
                GameManager.Instance.StartRestartCountdown();
                Destroy(gameObject);
            }
        }
    }
    void UpdateBounceText()
    {
        BounceCountText bounceCountText = FindFirstObjectByType<BounceCountText>();
        bounceCountText.UpdateBounceText(bulletDurability);
    }
}
