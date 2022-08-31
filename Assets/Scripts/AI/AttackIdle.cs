using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIdle : AIBehaviour
{
    //private AIController ourAI; // reference to the AI the owns the behaviour

    public float waitTimer;

    public override void Setup(AIController _AIController)
    {
        waitTimer = 0.5f;
        AI = _AIController;
        AI.ChangeAIAnimState(0);
        behaviourName = "AttackIdle";
    }

    public override void DoBehaviour()
    {
        //Debug.Log("Attack Idle");
        if (waitTimer <= 0.0f)
        {
            base.DoBehaviour(); // will either attack player again, move to attack player again, or will go back to patrol if player has been lost
        }
        waitTimer -= Time.deltaTime;
    }
}