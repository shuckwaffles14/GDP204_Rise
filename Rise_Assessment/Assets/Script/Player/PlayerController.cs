using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float moveDirection;
    BoxCollider2D bc2d;
    bool onGround, facingRight;
    Rigidbody2D rb2d;
    Transform t;
    Vector3 cameraPos;
    private Animator animator;
    static Vector2 targetLocation;

    [Header("Camera")]
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    Vector2 cameraOffset;

    [Header("Player movement stats (If setting in Unity Editor, comment out code in Start() in VS!)")]
    [SerializeField]
    static float health;
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float runSpeed;
    [SerializeField]
    float jumpHeight;
    [SerializeField]
    float groundDeceleration;
    [SerializeField]
    float fallMultiplier;
    [SerializeField]
    float lowJumpMultiplier;

    [Header("Ground Checker")]
    [SerializeField]
    public LayerMask groundLayer;
    [SerializeField]
    public Transform groundCheckerObj;
    [SerializeField]
    float checkerRadius;

    [Header("Orb Shooter")]
    [SerializeField]
    public Transform orbShooterObj;
    [SerializeField]
    Vector2 shooterOffset;
    [SerializeField]
    public GameObject fireball;
    [SerializeField]
    float attackCooldown;
    [SerializeField]
    static float fireballDamage;

    // Start is called before the first frame update
    void Start()
    {
        t = transform;
        health = 100.0f;
        bc2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        moveDirection = 0f;
        walkSpeed = 2.5f;
        runSpeed = 5f;
        facingRight = true;
        jumpHeight = 5f;
        onGround = false;
        fallMultiplier = 2.5f;
        lowJumpMultiplier = 2f;
        checkerRadius = 0.1f;
        animator = GetComponent<Animator>();
        attackCooldown = 0.1f;

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckForGround();
        Attack();
        Jump();
        BetterJump();
        Collisions();
        Camera();
    }

    void FixedUpdate()
    {

    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveBy = x * runSpeed;
            animator.SetFloat("Speed", 1.0f);
        }
        else
        {
            moveBy = x * walkSpeed;
            animator.SetFloat("Speed", 0.5f);
        }
        rb2d.velocity = new Vector2(moveBy, rb2d.velocity.y);

        if (x == 0f)
        {
            animator.SetFloat("Speed", 0.0f);
        }
    }

    private void Jump()
    {
        if (onGround)
        {
            Debug.Log("On Ground");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
                onGround = false;
                Debug.Log("Jump");
                animator.SetBool("Jumping", true);
            }
        }
    }

    private void BetterJump()
    {
        if (rb2d.velocity.y < 0) rb2d.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb2d.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) rb2d.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

    private void Camera()
    {
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x + cameraOffset.x, t.position.y + cameraOffset.y, cameraPos.z);
        }
    }

    private void CheckForGround()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundCheckerObj.position, checkerRadius, groundLayer);
        if (hit != null)
        {
            onGround = true;
            animator.SetBool("Jumping", false);
        }
        else onGround = false;
    }

    private void Collisions()
    {
        //Check for all collisions

        // Ladders

        // Enemies

        // Doors
    }

    public static void DoDamage(float damage)
    {
        health -= damage;
    }

    public static void AddHealth(float healthRefil)
    {
        health += healthRefil;
    }

    void HealthCheck()
    {
        if(health <= 0f)
        {
            //die
        }
    }

    void Attack()
    {
        /*if (attackCooldown < 0)*/ attackCooldown -= Time.deltaTime;
        if (Input.GetKey(KeyCode.T) && attackCooldown <= 0f)
        {
            GameObject clone;
            clone = Instantiate(fireball, orbShooterObj);
            GetTarget(clone.GetComponent<FireballController>().fireballRange);
            clone.GetComponent<Rigidbody2D>().velocity = targetLocation * clone.GetComponent<FireballController>().fireballSpeed;
            Debug.DrawRay(orbShooterObj.position, targetLocation, Color.yellow, 2f);
            Destroy(clone, 2.5f);
            Debug.Log("Fireball!");
            animator.SetBool("Attacking", true);
            attackCooldown = 0.19f;
        }
        if (attackCooldown <= 0.0f)
        {
            animator.SetBool("Attacking", false);
        }
    }

    void GetTarget(float range)
    {
        Vector2 myPos = t.position;
        targetLocation = (Vector2.left) * range;
    }

    public Vector2 FireballTarget()
    {
        return targetLocation;
    }

    public bool FireballDirection()
    {
        return facingRight;
    }

    public float GetFireballDamage()
    {
        return fireballDamage;
    }
}
