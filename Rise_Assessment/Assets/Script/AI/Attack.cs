using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : AIBehaviour
{
    //private AIController ourAI; // reference to the AI the owns the behaviour
    float attackTimer; // how long the attack actually lasts

    public override void Setup(AIController _AIController)
    {
        AI = _AIController;
        AI.ChangeAIAnimState(2);
        attackTimer = 0.25f;
    }

    public override void DoBehaviour()
    {
        if (attackTimer <= 0.0f)
        {
            AI.NewTopState(new AttackIdle());
        }
        attackTimer -= Time.deltaTime;
    }
}
