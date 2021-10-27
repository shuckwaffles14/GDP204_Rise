using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : AIBehaviour
{
    private float waitTimer;
    // Start is called before the first frame update
    void Start()
    {
        waitTimer = 2.5f;
        AI.ChangeAIAnimState(0);
    }

    public override void DoBehaviour()
    {
        base.DoBehaviour(); // check for player

        if (waitTimer <= 0.0f)
        {
            waitTimer = 2.5f;
            AI.AddState(new Patrol()); // Adds a patrol state if no player found
        }
        waitTimer -= Time.deltaTime;
    }
}
