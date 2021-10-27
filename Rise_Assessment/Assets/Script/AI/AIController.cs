using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 currentTarget;
    private AIStateMachine stateMachine;
    private bool direction; // false = left, true = right
    private int currentCheckpoint;
    private Animator animator;
    private SpriteRenderer sr;

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

    // Start is called before the first frame update
    void Start()
    {
        if(!useEditorVariables)
        {
            movementSpeed = 1.0f;
            checkpointSensitivity = 0.1f;
        }
        rb2d = GetComponent<Rigidbody2D>();
        stateMachine = new AIStateMachine(this);
        currentCheckpoint = 0;
        currentTarget.x = checkpoints[currentCheckpoint].transform.position.x;
        currentTarget.y = checkpoints[currentCheckpoint].transform.position.y;
        aiAS = AIAnimState.Idle;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Animator();
        if (direction) sr.flipX = true; // facing/moving right
        else sr.flipX = false; // facing/moving left
        stateMachine.DoWork();
    }

    void Animator()
    {
        switch (aiAS)
        {
            case AIAnimState.Idle:
                animator.SetInteger("AnimationState", 0);
                break;
            case AIAnimState.Walking:
                animator.SetInteger("AnimationState", 1);
                break;
            case AIAnimState.Attacking:
                animator.SetInteger("AnimationState", 2);
                break;
            case AIAnimState.Death:
                animator.SetInteger("AnimationState", 3);
                break;
        }    
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
        currentCheckpoint++;
        int cpSize = checkpoints.Length;
        if (currentCheckpoint + 1 >= cpSize)
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
            rb2d.velocity = new Vector2((1 * movementSpeed), rb2d.velocity.y);
        }
        else // moving left
        {
            rb2d.velocity = new Vector2((-1 * movementSpeed), rb2d.velocity.y);
        }
    }
}
