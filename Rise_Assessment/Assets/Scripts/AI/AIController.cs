﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 currentTarget;
    private bool direction; // false = left, true = right
    private int currentCheckpoint;
    private static float health;
    private const float attackColliderOffset = 5.25f;

    public SpriteRenderer sr;
    public Animator animator;
    public AIStateMachine stateMachine;
    public BoxCollider2D attackCollider;

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
    [SerializeField]
    float attackKnockbackForce;

    // Start is called before the first frame update
    void Start()
    {
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
        attackCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(health);
        //Debug.DrawRay(transform.position, Vector2.left, Color.yellow, checkpointSensitivity);
        Animator();
        if (direction) // facing/moving right
        {
            sr.flipX = true;
            attackCollider.offset = new Vector2(attackColliderOffset, -5);
        }
        else // facing/moving left
        {
            sr.flipX = false;
            attackCollider.offset = new Vector2(-1 * attackColliderOffset, -5);
        }
        //stateMachine.DoWork();
    }

    //private void OnCollisionEnter2D(Collision2D collision) // Checks normal hitbox for AI in case player runs into enemy
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        collision.gameObject.GetComponent<PlayerController>().DoDamage(attackDamage);
    //        if (direction)
    //        {
    //            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 100);
    //            collision.gameObject.GetComponent<Rigidbody2D>().AddForce((transform.right * 1000) * attackKnockbackForce);
    //        }
    //        else if (!direction)
    //        {
    //            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 100);
    //            collision.gameObject.GetComponent<Rigidbody2D>().AddForce((transform.right * -1000) * attackKnockbackForce);
    //        }
    //        //collision.gameObject.GetComponent<PlayerController>().Knockback(transform.position, attackKnockbackForce);
    //        NewTopState(new AttackIdle());
    //        //RemoveState(); // Go back to idle
    //    }
    //}

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
        if (direction) // moving right
        {
            rb2d.velocity = Vector2.right * movementSpeed;
            //transform.position += (Vector3.right * movementSpeed) * Time.fixedDeltaTime;
            //Debug.Log("AI move right");
        }
        else // moving left
        {
            rb2d.velocity = Vector2.left * movementSpeed;
            //transform.position += (Vector3.left * movementSpeed) * Time.fixedDeltaTime;
            //Debug.Log("AI move left");
        }
    }

    public void DoDamage(float damage)
    {
        health -= damage;
        Debug.Log(damage + " done to AI");
        Debug.Log("AI health = " + health);
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }

    public void Knockback(Vector2 attackerPos, float attackForce)
    {
        Vector2 myPos;
        myPos.x = transform.position.x;
        myPos.y = transform.position.y;

        Vector2 force = (attackerPos - myPos).normalized * attackForce;
        GetComponent<Rigidbody2D>().AddForce(force);
    }
}