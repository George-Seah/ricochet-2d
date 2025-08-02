using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerBehaviour : MonoBehaviour
{
    bool jumpPressed;
    bool grounded;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpTime = .3f;
    [SerializeField] float initialJumpForce;
    float jumpForce;
    [SerializeField] float normalGravityScale;
    [SerializeField] float fallingGravityScale;
    [SerializeField] Rigidbody2D gunPivot;
    SpriteRenderer gunSprite;
    [SerializeField] Transform gunTip;
    [SerializeField] GameObject bullet;
    public int maxBulletBounces;
    Rigidbody2D rb;

    InputActionAsset inputActionAsset;
    InputAction move;
    InputAction jump;
    InputAction shoot;
    InputAction aim;
    InputAction pause;

    Vector2 moveInput;
    bool shootPressed;
    Vector2 aimInput;
    Camera mainCamera;
    bool hasShot = false;
    float finalAngle;
    Vector2 aimDirection;
    void Awake()
    {
        inputActionAsset = GetComponent<PlayerInput>().actions;
        rb = GetComponent<Rigidbody2D>();

        gunSprite = gunPivot.GetComponentInChildren<SpriteRenderer>();

        move = inputActionAsset.FindAction("Move");
        jump = inputActionAsset.FindAction("Jump");
        shoot = inputActionAsset.FindAction("Shoot");
        aim = inputActionAsset.FindAction("Aim");
        pause = inputActionAsset.FindAction("Pause");

        mainCamera = Camera.main;
    }

    void OnEnable()
    {
        move.Enable();
        jump.Enable();
        shoot.Enable();
        aim.Enable();
        pause.Enable();

        jump.performed += OnJump;
        jump.canceled += OnJumpCanceled;

        move.performed += OnMove;
        move.canceled += OnMoveCanceled;

        shoot.performed += OnShoot;
        shoot.canceled += OnShootCanceled;

        aim.performed += OnAim;
        aim.canceled += OnAimCanceled;

        pause.performed += OnPause;
        //pause.canceled += OnPauseCanceled;
    }

    void OnDisable()
    {
        move.Disable();
        jump.Disable();
        aim.Disable();
        shoot.Disable();
        pause.Disable();

        jump.performed -= OnJump;
        jump.canceled -= OnJumpCanceled;

        move.performed -= OnMove;
        move.canceled -= OnMoveCanceled;

        shoot.performed -= OnShoot;
        shoot.canceled -= OnShootCanceled;

        aim.performed -= OnAim;
        aim.canceled -= OnAimCanceled;

        pause.performed -= OnPause;
        //pause.canceled -= OnPauseCanceled;
    }

    public void OnJump(InputAction.CallbackContext context) { jumpPressed = true; }

    public void OnJumpCanceled(InputAction.CallbackContext context)
    {
        jumpPressed = false;
        jumpForce = 0;
        jumpTime = 0;
        rb.gravityScale = fallingGravityScale;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        // You can now use moveInput in Update or FixedUpdate
    }
    public void OnMoveCanceled(InputAction.CallbackContext context)
    { moveInput = new Vector2(0f, 0f); }

    public void OnAim(InputAction.CallbackContext context)
    {
        aimInput = context.ReadValue<Vector2>();
        aimDirection = aimInput;
    }
    public void OnAimCanceled(InputAction.CallbackContext context)
    { aimInput = new Vector2(0f, 0f); }
    public void OnShoot(InputAction.CallbackContext context) { shootPressed = true; }

    public void OnShootCanceled(InputAction.CallbackContext context) { shootPressed = false; }

    public void OnPause(InputAction.CallbackContext context)
    {
        GameManager.Instance.Pause();
    }


    void FixedUpdate()
    {
        //Movement
        Vector2 movement = new Vector2(moveInput.x, 0f);
        rb.linearVelocity = movement * moveSpeed;


        //Targeting
        
        //Vector2 screenPos = aimInput;
        //Vector2 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        || (Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            aimDirection = (worldPos - (Vector2)transform.position).normalized;
        }

        //Debug.Log(worldPos);


        finalAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //gunPivot.localRotation = Quaternion.Euler(0f, 0f, finalAngle);
        gunPivot.MoveRotation(finalAngle);
        // 0 - 90 = right; -90 - 0 = right; 90 - 180 = left; - 180 - -90 = right
        bool facingRight = finalAngle > -90f && finalAngle < 90f;
        Debug.Log(facingRight);
        gunSprite.flipY = !facingRight;
        //Jumping
        if (jumpPressed && grounded)
        {
            jumpForce = initialJumpForce;

            Debug.Log("OnJump function activating gravity.");
        }
        if (jumpPressed && jumpTime > 0)
        {
            jumpTime -= Time.fixedDeltaTime;
            jumpForce *= .9f;
            rb.linearVelocityY += jumpForce;
            
        }
        else if (!grounded && (jumpTime <= 0 || rb.linearVelocityY <= 0)) {
            rb.gravityScale = fallingGravityScale;
        }


        //Shooting
        if (shootPressed) ShootBullet();
    }

    void ShootBullet()
    {
        if (hasShot) return;
        Bullet newBullet = Instantiate(bullet, gunTip.position, Quaternion.Euler(0f, 0f, finalAngle - 90f)).GetComponent<Bullet>();
        newBullet.bulletDurability = maxBulletBounces;

        WorldScript newWorldScript = newBullet.GetComponent<WorldScript>();
        newWorldScript.world1 = GetComponent<WorldScript>().world1;
        newWorldScript.SetWorldLayer();

        Physics2D.IgnoreCollision(newBullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        hasShot = true;
    }

    void HandleTriggerEnterStay(Collider2D other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("World Changer") || other.gameObject.CompareTag("Target")) return;
        if (!other.gameObject.CompareTag("Ground")) return;
        grounded = true;
        rb.gravityScale = normalGravityScale;
        jumpTime = .3f;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        HandleTriggerEnterStay(other);
    }
    void OnTriggerStay2D(Collider2D other)
    { 
        HandleTriggerEnterStay(other);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground")) grounded = false;
    }
}
