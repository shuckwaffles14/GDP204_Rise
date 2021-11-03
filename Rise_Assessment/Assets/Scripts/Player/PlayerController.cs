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

    [Header("Player movement stuff")]
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
    public Transform leftOrbShooterObj;
    [SerializeField]
    public Transform rightOrbShooterObj;
    [SerializeField]
    Vector2 shooterOffset;
    [SerializeField]
    public GameObject fireball;
    [SerializeField]
    float attackCooldown;
    [SerializeField]
    float fireballDamage;

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
        //Debug.Log(facingRight);
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
        else if (x > 0f)
        {
            facingRight = true;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (x < 0f)
        {
            facingRight = false;
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void Jump()
    {
        if (onGround)
        {
            //Debug.Log("On Ground");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
                onGround = false;
                //Debug.Log("Jump");
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


        // Or do a collision func for collisions
    }

    public void DoDamage(float damage)
    {
        health -= damage;
    }

    public void AddHealth(float healthRefil) //add or take health, use negative value for takign health (e.g. poison potion)
    {
        health += healthRefil;
        //Debug.Log(health);
    }

    void HealthCheck()
    {
        if (health <= 0f)
        {
            //die
        }
    }

    void Attack()
    {
        // Get target here

        /*if (attackCooldown < 0)*/
        attackCooldown -= Time.deltaTime;
        if (Input.GetKey(KeyCode.T) && attackCooldown <= 0f)
        {
            GameObject clone;

            if (facingRight)
            {
                clone = Instantiate(fireball, rightOrbShooterObj);
            }
            else
            {
                clone = Instantiate(fireball, leftOrbShooterObj);
            }
            
            GetTarget(clone.GetComponent<FireballController>().fireballRange);
            clone.GetComponent<Rigidbody2D>().velocity = targetLocation * clone.GetComponent<FireballController>().fireballSpeed;
            //Debug.DrawRay(orbShooterObj.position, targetLocation, Color.yellow, 2f);
            Destroy(clone, 2.5f);
            //Debug.Log("Fireball!");

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
        // add checks for ai enemies using raycasts
        if (facingRight) targetLocation = Vector2.right * range;
        else targetLocation = (Vector2.left) * range;
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