using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // CHANGE THIS

public class PlayerController : MonoBehaviour
{
    float moveDirection;
    BoxCollider2D bc2d;
    bool onGround, facingRight, playerDead, canAttack;
    Rigidbody2D rb2d;
    Transform t;
    Vector3 cameraPos;
    private Animator animator;
    static Vector2 targetLocation;
    private GameObject pauseObj;
    float invulnerability;

    public Slider healthBar;
    
    [Header("Camera")]
    [SerializeField]
    GameObject mainCamera;
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
    float climbSpeed;
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
    [SerializeField]
    float attackKnockbackForce;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
        t = transform;
        health = 100.0f;
        bc2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        moveDirection = 0f;
        walkSpeed = 2.5f;
        runSpeed = 5f;
        facingRight = true;
        jumpHeight = 7.5f;
        onGround = false;
        fallMultiplier = 2.5f;
        lowJumpMultiplier = 2f;
        checkerRadius = 0.1f;
        animator = GetComponent<Animator>();
        attackCooldown = 0.1f;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        pauseObj = GameObject.FindGameObjectWithTag("Pause");
        playerDead = false;
        invulnerability = 0f;
        climbSpeed = 2f;
        healthBar = GameObject.FindWithTag("HealthBar").GetComponent<Slider>();
        healthBar.value = health;

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Player health = " + health);
        if (!playerDead)
        {
            if (!pauseObj.GetComponent<PauseMenu>().GetPauseStatus())
            {
                Debug.Log(onGround);
                invulnerability -= Time.deltaTime;
                HealthCheck();
                Move();
                CheckForGround();
                Attack();
                Jump();
                BetterJump();
                //Collisions();
                Camera();
            }
        }
        else
        {
            rb2d.velocity = new Vector2(0f, 0f);
            if (Input.anyKey)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload scene on death
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            rb2d.gravityScale = 0f;
            canAttack = false;
            float horizontalInput = Input.GetAxis("Horizontal");
            if (Input.GetKey(KeyCode.W) || horizontalInput > 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, climbSpeed);
                animator.SetBool("Climbing", true);
            }
            else if (Input.GetKey(KeyCode.S) || horizontalInput < 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, -1 * climbSpeed);
                animator.SetBool("Climbing", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            canAttack = true;
            rb2d.gravityScale = 1f;
            animator.SetBool("Climbing", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" && transform.position.y > collision.transform.position.y)
        {
            Debug.Log("On Platform");
            onGround = true;
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy;
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift)) || (Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Joystick1Button2)))
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
            float verticalInput = Input.GetAxis("Vertical");
            if (Input.GetKeyDown(KeyCode.Space) || verticalInput > 0 || Input.GetKeyDown(KeyCode.Joystick1Button1))
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
        else if (rb2d.velocity.y > 0 && (!Input.GetKey(KeyCode.Space) || !Input.GetKey(KeyCode.Joystick1Button1))) rb2d.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

    private void Camera()
    {
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x + cameraOffset.x, t.position.y + cameraOffset.y, cameraPos.z);
        }
    }

    public void CheckForGround()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundCheckerObj.position, checkerRadius, groundLayer);
        if (hit != null)
        {
            onGround = true;
            animator.SetBool("Jumping", false);
        }
        else onGround = false;
    }

    public void DoDamage(float damage)
    {
        if (invulnerability <= 0.00f)
        {
            health -= damage;
            invulnerability = 0.25f;
            Debug.Log(damage + " done to player");
            Debug.Log("AI health = " + health);
        }
    }

    public void AddHealth(float healthRefil) //add or take health, use negative value for takign health (e.g. poison potion)
    {
        health += healthRefil;
        //Debug.Log(health);
    }

    public void Knockback(Vector2 attackerPos, float attackForce)
    {
        Vector2 myPos;
        myPos.x = transform.position.x;
        myPos.y = transform.position.y;

        Vector2 force = (attackerPos - myPos).normalized * attackForce;
        GetComponent<Rigidbody2D>().AddForce(force);
    }

    void HealthCheck()
    {
        healthBar.value = health;
        if (health <= 0f)
        {
            animator.SetTrigger("PlayerDeath");
            playerDead = true;
            rb2d.gravityScale = 1f; // makes sure that if player dies on ladder, gravity gets returned to normal
        }
    }

    void Attack()
    {
        // Get target here

        /*if (attackCooldown < 0)*/
        attackCooldown -= Time.deltaTime;
        if ((Input.GetKey(KeyCode.T) || Input.GetKey(KeyCode.Joystick1Button0)) && attackCooldown <= 0f)
        {
            GameObject clone;

            if (facingRight)
            {
                clone = Instantiate(fireball, rightOrbShooterObj);
                clone.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                clone = Instantiate(fireball, leftOrbShooterObj);
                clone.GetComponent<SpriteRenderer>().flipX = false;
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

    public float GetKnockbackForce()
    {
        return attackKnockbackForce;
    }

    public Vector2 GetPos()
    {
        Vector2 myPos;
        myPos.x = transform.position.x;
        myPos.y = transform.position.y;
        return myPos;
    }

    public bool GetPlayerDead()
    {
        return playerDead;
    }

    public void SetOnGround(bool _onGround)
    {
        onGround = _onGround;
    }
}