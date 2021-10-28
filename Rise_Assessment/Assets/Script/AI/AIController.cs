using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 currentTarget;
    private bool direction; // false = left, true = right
    private int currentCheckpoint;
    private float health;

    public SpriteRenderer sr;
    public Animator animator;
    public AIStateMachine stateMachine;

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
    public GameObject[] checkpoints;// patrol checkpoints for ai - must be set in Unity
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

    // Start is called before the first frame update
    void Start()
    {
        if(!useEditorVariables)
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
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.left, Color.yellow, checkpointSensitivity);
        Animator();
        if (direction) sr.flipX = true; // facing/moving right
        else sr.flipX = false; // facing/moving left
        //stateMachine.DoWork();
    }

    void Animator()
    {
        if (aiAS == AIAnimState.Idle) animator.SetInteger("AnimationState", 0);
        else if (aiAS == AIAnimState.Walking) animator.SetInteger("AnimationState", 1);
        else if (aiAS == AIAnimState.Attacking) animator.SetInteger("AnimationState", 2);
        else if (aiAS == AIAnimState.Death) animator.SetInteger("AnimationState", 3);
    }

    public Vector2 GetCurrentTarget()
    {
        return currentTarget;
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
        Debug.Log("Get Next Checkpoint");
        currentCheckpoint++;
        int cpSize = checkpoints.Length;
        if (currentCheckpoint == cpSize )
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
        if (direction) // moving right
        {
            rb2d.velocity = Vector2.right * movementSpeed;
            //transform.position += (Vector3.right * movementSpeed) * Time.fixedDeltaTime;
            Debug.Log("AI move right");
        }
        else // moving left
        {
            rb2d.velocity = Vector2.left * movementSpeed;
            //transform.position += (Vector3.left * movementSpeed) * Time.fixedDeltaTime;
            Debug.Log("AI move left");
        }
    }

    public void DoDamage(float damage)
    {
        health -= damage;
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }
}
