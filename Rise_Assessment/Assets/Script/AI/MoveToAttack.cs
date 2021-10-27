using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAttack : AIBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AI.ChangeAIAnimState(1);
    }

    public override void DoBehaviour()
    {
        base.DoBehaviour(); // check for player

        Vector2 target = AI.GetCurrentTarget();
        if (Vector2.Distance(target, AI.transform.position) > AI.attackRange) // if ai is out of attack range
        {
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
