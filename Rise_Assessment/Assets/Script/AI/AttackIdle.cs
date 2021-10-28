using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIdle : AIBehaviour
{
    //private AIController ourAI; // reference to the AI the owns the behaviour

    public float waitTimer;

    public override void Setup(AIController _AIController)
    {
        waitTimer = 0.2f;
        AI = _AIController;
        AI.ChangeAIAnimState(0);
    }

    public override void DoBehaviour()
    {
        if (waitTimer <= 0.0f)
        {
            base.DoBehaviour(); // will either attack player again, move to attack player again, or will go back to idle to then go back to patrol if player has been lost
        }
        waitTimer -= Time.deltaTime;
    }
}
