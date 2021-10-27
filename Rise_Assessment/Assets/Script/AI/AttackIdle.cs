using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIdle : AIBehaviour
{
    public float waitTimer;
    // Start is called before the first frame update
    void Start()
    {
        waitTimer = 0.2f;
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
