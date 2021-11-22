using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : AIBehaviour
{
    //private AIController ourAI; // reference to the AI the owns the behaviour
    float attackTimer; // how long the attack actually lasts
    int enemyType; // 1 = E1, 2 = E2, 3 = E3
    bool attacked;

    public override void Setup(AIController _AIController)
    {
        AI = _AIController;
        AI.ChangeAIAnimState(2);
        behaviourName = "Attack";
        enemyType = AI.GetEnemyType();
        if (enemyType == 1)
        {
            AI.attackCollider.enabled = true; // turn on collider for AI attacks
            attackTimer = 0.25f;
        }
        else if (enemyType == 2)
        {
            attackTimer = 1.5f;
        }
    }

    public override void DoBehaviour()
    {
        Debug.Log("Attack");
        if (enemyType == 1)
        {
            if (attackTimer <= 0.0f)
            {
                attacked = true;
                AI.attackCollider.enabled = false; // turn collider back off to stop attacks (E1)
                AI.NewTopState(new AttackIdle());
            }
            attackTimer -= Time.deltaTime;
        }
        else if (enemyType == 2)
        {
            if (attackTimer <= 0.66f && !attacked)
            {
                GameObject clone;
                attacked = true;
                if (AI.GetDirection())
                {
                    clone = GameObject.Instantiate(AI.fireball, AI.eyesRight.transform);
                    clone.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    clone = GameObject.Instantiate(AI.fireball, AI.eyesLeft.transform);
                    clone.GetComponent<SpriteRenderer>().flipX = false;
                }
                clone.GetComponent<EnemyProjectileController>().AI = AI.gameObject;
                Debug.Log(AI.GetCurrentTarget());
                Debug.Log(AI.GetPlayerPos());
                //clone.GetComponent<Rigidbody2D>().velocity = AI.GetPlayerPos() * clone.GetComponent<EnemyProjectileController>().projectileSpeed;
            }
            if (attackTimer <= 0.0f) AI.NewTopState(new AttackIdle());
            attackTimer -= Time.deltaTime;
        }
    }
}