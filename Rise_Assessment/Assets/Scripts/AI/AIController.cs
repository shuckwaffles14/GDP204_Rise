using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 currentTarget;
    private bool direction; // false = left, true = right
    private int currentCheckpoint;
    private int enemyType; // 1 = E1, 2 = E2, 3 = E3
    private float health;
    private const float attackColliderOffset = 5.25f;
    private GameObject pauseObj;
    float invulnerability;
    float distanceToGround;
    private GameObject player;

    public SpriteRenderer sr;
    public Animator animator;
    public AIStateMachine stateMachine;
    [Tooltip("This is only necessary for E1")]
    public BoxCollider2D attackCollider;
    [Tooltip("This is only necessary for E2")]
    public GameObject fireball;

    private enum AIAnimState
    {
        Idle,
        Walking,
        Attacking,
        Death
    }

    AIAnimState aiAS;

    [Header("AI movement stuff")]
    [SerializeField]
    bool useEditorVariables;
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    float checkpointSensitivity;
    [SerializeField]
    public GameObject[] checkpoints;// patrol checkpoints for ai
    [SerializeField]
    public GameObject eyesLeft;
    [SerializeField]
    public GameObject eyesRight;
    [SerializeField]
    float viewDistance;
    [SerializeField]
    public float attackRange;
    [SerializeField]
    float attackDamage;
    [SerializeField]
    float attackKnockbackForce;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
        Debug.Log("Start of Start()");
        if (!useEditorVariables)
        {
            movementSpeed = 2.5f;
            checkpointSensitivity = 0.5f;
        }
        rb2d = GetComponent<Rigidbody2D>();
        //stateMachine = new AIStateMachine(this);
        currentCheckpoint = 0;
        currentTarget.x = checkpoints[currentCheckpoint].transform.position.x;
        currentTarget.y = checkpoints[currentCheckpoint].transform.position.y;
        aiAS = AIAnimState.Idle;
        //animator = GetComponent<Animator>();
        //sr = GetComponent<SpriteRenderer>();
        health = 100.0f;
        if (enemyType == 1) attackCollider.enabled = false;
        pauseObj = GameObject.FindGameObjectWithTag("Pause");
        invulnerability = 0f;
        Debug.Log("End of Start()");
        distanceToGround = 100f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(health);
        //Debug.DrawRay(transform.position, Vector2.left, Color.yellow, checkpointSensitivity);
        if (!pauseObj.GetComponent<PauseMenu>().GetPauseStatus())
        {
            invulnerability -= Time.deltaTime;
            Animator();
            if (direction) // facing/moving right
            {
                sr.flipX = true;
                if (enemyType == 1) attackCollider.offset = new Vector2(attackColliderOffset, -5);
            }
            else // facing/moving left
            {
                sr.flipX = false;
                if (enemyType == 1) attackCollider.offset = new Vector2(-1 * attackColliderOffset, -5);
            }

            if (health <= 0f)
            {
                NewTopState(new Death());
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                GetComponent<Rigidbody2D>().gravityScale = 0f;
                BoxCollider2D[] bc2dList = GetComponents<BoxCollider2D>();
                foreach (BoxCollider2D bc2d in bc2dList)
                {
                    bc2d.enabled = false;
                }
                CircleCollider2D cc2d = GetComponent<CircleCollider2D>();
                cc2d.enabled = false;
                Destroy(this.gameObject, 3f);
            }
        }
        //stateMachine.DoWork();
    }

    private void OnCollisionEnter2D(Collision2D collision) // Checks normal hitbox for AI in case player runs into enemy
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().DoDamage(attackDamage);
            if (direction)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 100);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce((transform.right * 1000) * attackKnockbackForce);
            }
            else if (!direction)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 100);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce((transform.right * -1000) * attackKnockbackForce);
            }
            //collision.gameObject.GetComponent<PlayerController>().Knockback(transform.position, attackKnockbackForce);
            NewTopState(new AttackIdle());
            //RemoveState(); // Go back to idle
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Checks the attack hitbox while active
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().DoDamage(attackDamage);
            if (direction)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 100);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce((transform.right * 1000) * attackKnockbackForce);
            }
            else if (!direction)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 100);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce((transform.right * -1000) * attackKnockbackForce);
            }
            //collision.gameObject.GetComponent<PlayerController>().Knockback(transform.position, attackKnockbackForce);
            attackCollider.enabled = false;
            NewTopState(new AttackIdle());
        }
    }

    public void Setup(GameObject[] _checkpoints, int eType)
    {
        checkpoints = _checkpoints;
        enemyType = eType;
    }

    void Animator()
    {
        if (aiAS == AIAnimState.Idle) animator.SetInteger("AnimationState", 0);
        else if (aiAS == AIAnimState.Walking) animator.SetInteger("AnimationState", 1);
        else if (aiAS == AIAnimState.Attacking) animator.SetInteger("AnimationState", 2);
        else if (aiAS == AIAnimState.Death) animator.SetTrigger("Dead");
    }

    public Vector2 GetCurrentTarget()
    {
        return currentTarget;
    }

    public Vector2 GetPlayerPos() // is only used by E2 when it is already attacking
    {
        //Vector3 location = player.transform.parent.parent.InverseTransformPoint;
        Vector3 tempLocation = player.transform.InverseTransformPoint(player.transform.position);
        Vector2 location = new Vector2();
        location.x = tempLocation.x;
        location.y = tempLocation.y;
        return location;
    }

    public float GetCheckpointSensitivity()
    {
        return checkpointSensitivity;
    }

    public void AddState(AIBehaviour state)
    {
        stateMachine.AddState(state);
    }

    public void RemoveState()
    {
        stateMachine.PopState();
    }

    public void NewTopState(AIBehaviour state)
    {
        stateMachine.NewTopState(state);
    }

    public AIBehaviour GetTopState()
    {
        return stateMachine.GetTopState();
    }

    public bool GetDirection()
    {
        return direction;
    }

    public float GetViewDistance()
    {
        return viewDistance;
    }

    public void GetNextCheckpoint()
    {
        //Debug.Log("Get Next Checkpoint");
        currentCheckpoint++;
        int cpSize = checkpoints.Length;
        if (currentCheckpoint == cpSize)
        {
            // reset the checkpoint to the first in list
            currentCheckpoint = 0;
        }

        currentTarget.x = checkpoints[currentCheckpoint].transform.position.x;
        currentTarget.y = checkpoints[currentCheckpoint].transform.position.y;
    }

    public void UpdateTarget(Vector2 newTarget)
    {
        currentTarget = newTarget;
    }

    public void ChangeAIAnimState(int animNum) // 0 = idle, 1 = walking, 2 = attacking, 3 = dead
    {
        switch (animNum)
        {
            case 0:
                aiAS = AIAnimState.Idle;
                break;
            case 1:
                aiAS = AIAnimState.Walking;
                break;
            case 2:
                aiAS = AIAnimState.Attacking;
                break;
            case 3:
                aiAS = AIAnimState.Death;
                break;
        }
    }

    public void Move(bool _direction)
    {
        direction = _direction;
        if (CheckNextMovement(direction))
        {
            if (direction) // moving right
            {
                rb2d.velocity = Vector2.right * movementSpeed;
            }
            else // moving left
            {
                rb2d.velocity = Vector2.left * movementSpeed;
            }
        }
        else
        {
            Vector2 curTar = GetCurrentTarget();
            foreach(GameObject cp in checkpoints)
            {
                Vector2 tempCPPos = cp.transform.position;
                if (curTar == tempCPPos) // if the current targetted checkpoint is not accessible, move the location to this point
                {
                    Vector2 thisPos = transform.position;
                    cp.transform.position = thisPos;
                }
            }

            Vector2 tempPlayerPos = player.transform.position;
            if (curTar == tempPlayerPos)
            {
                // change direction
                if (direction) direction = false;
                else direction = true;

                // go back to idle
                GetNextCheckpoint();
                RemoveState();
            }
        }
    }

    private bool CheckNextMovement(bool _direction) // Checks if AI is about to walk off a ledge. If AI is about to walk off a ledge to reach next checkpoint, move that checkpoint to this position
    {
        RaycastHit2D hit;
        Vector2 rayDir = Vector2.down;
        if (_direction)
        {
            rayDir.x = 0.05f;
            hit = Physics2D.Raycast(eyesRight.transform.position, rayDir, distanceToGround);
        }
        else
        {
            rayDir.x = -0.05f;
            hit = Physics2D.Raycast(eyesLeft.transform.position, rayDir, distanceToGround) ;
        }

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player" || hit.collider.tag == "Collisions")
            {
                return true;
            }
        }

        return false;
    }

    public float GetHealth()
    {
        return health;
    }

    public void DoDamage(float damage)
    {
        if (invulnerability <= 0.00f)
        {
            health -= damage;
            invulnerability = 0.25f;
        }
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }

    public float GetKnockbackForce()
    {
        return attackKnockbackForce;
    }

    public void Knockback(Vector2 attackerPos, float attackForce)
    {
        Vector2 myPos;
        myPos.x = transform.position.x;
        myPos.y = transform.position.y;

        Vector2 force = (myPos - attackerPos).normalized * attackForce;
        GetComponent<Rigidbody2D>().AddForce(force + (Vector2.up * 100f));
    }

    public int GetEnemyType()
    {
        return enemyType;
    }

    public Vector2 GetPos()
    {
        Vector2 myPos;
        myPos.x = transform.position.x;
        myPos.y = transform.position.y;
        return myPos;
    }
}