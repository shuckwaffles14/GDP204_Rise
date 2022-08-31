using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : AIBehaviour
{
    private float waitTimer;

    //private AIController ourAI; // reference to the AI the owns the behaviour

    public override void Setup(AIController _AIController)
    {
        waitTimer = 2.5f;
        AI = _AIController;
        AI.ChangeAIAnimState(0);
        behaviourName = "Idle";
        //Debug.Log("Idle Setup");
    }

    public override void DoBehaviour()
    {
        base.DoBehaviour(); // check for player
        //Debug.Log("Idle");
        if (waitTimer <= 0.0f)
        {
            AI.AddState(new Patrol()); // Adds a patrol state if no player found
        }
        waitTimer -= Time.deltaTime;
    }

    public override void ResetTimer()
    {
        waitTimer = 2.5f;
        AI.ChangeAIAnimState(0);
    }
}