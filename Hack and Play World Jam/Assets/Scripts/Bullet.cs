using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed = 15f;
    //[SerializeField] float raycastLength = 5f;
    Rigidbody2D rb;
    public int bulletDurability;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Initialize velocity from rotation
        //rb.linearVelocity = transform.up * moveSpeed;
        Debug.Log($"Bullet durability is {bulletDurability}");

        //rb.AddRelativeForce(transform.up * moveSpeed, ForceMode2D.Force);
    }

    void FixedUpdate()
    {
        // Every physics step, velocity follows current rotation
        rb.linearVelocity = rb.transform.up * moveSpeed;

        //! Raycast stuff
        /*
        RaycastHit hit;
        
        if (Physics.Raycast(rb.transform.position, transform.up, out hit, raycastLength))
        {
            Debug.DrawRay(transform.position, transform.up * raycastLength, Color.yellow);
            if (hit.gameObject.CompareTag("Ground")) {

            }
        }
        */
        //rb.AddRelativeForce(transform.up * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
        //rb.AddRelativeForce(transform.up * moveSpeed, ForceMode2D.Impulse);

        //rb.AddRelativeForceY(moveSpeed * Time.fixedDeltaTime);
        //rb.AddRelativeForceY(moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
        //rb.AddRelativeForceY(moveSpeed, ForceMode2D.Force, ForceMode2D.Impulse);

        //rb.MovePosition((rb.position + Vector2.up) * moveSpeed * Time.fixedDeltaTime);
    }

    void HandleCollision(Collision2D collision, bool damageBullet)
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
            Vector2 newDir = Vector2.Reflect(rb.transform.up, normal).normalized;

            // Apply it by rotating the transform—this automatically changes velocity next FixedUpdate
            float newAngle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg - 90f;
            rb.transform.rotation = Quaternion.Euler(0f, 0f, newAngle);

            if (!damageBullet) return;
            
            bulletDurability--;
            UpdateBounceText();
            if (bulletDurability <= 0 && GameObject.FindGameObjectsWithTag("Target").Length > 0)
            {
                GameManager.Instance.StartRestartCountdown();
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision, true);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollision(collision, false);
    }

    void UpdateBounceText()
    {
        BounceCountText bounceCountText = FindFirstObjectByType<BounceCountText>();
        bounceCountText.UpdateBounceText(bulletDurability);
    }
}
