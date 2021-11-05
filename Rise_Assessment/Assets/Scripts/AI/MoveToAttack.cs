using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAttack : AIBehaviour
{
    //private AIController ourAI; // reference to the AI the owns the behaviour

    public override void Setup(AIController _AIController)
    {
        AI = _AIController;
        AI.ChangeAIAnimState(1);
        behaviourName = "MoveToAttack";
    }

    public override void DoBehaviour()
    {
        base.DoBehaviour(); // check for player

        //Debug.Log("Move to Attack");

        Vector2 target = AI.GetCurrentTarget();
        if (Vector2.Distance(target, AI.transform.position) > AI.attackRange) // if ai is out of attack range
        {
            //Debug.Log("distance = " + Vector2.Distance(target, AI.transform.position).ToString());
            //Debug.Log(AI.attackRange);
            if (target.x >= AI.transform.position.x)
            {
                AI.Move(true); // move right
            }
            else
            {
                AI.Move(false); // move left
            }
        }
        else
        {
            AI.NewTopState(new Attack());
        }
    }
}