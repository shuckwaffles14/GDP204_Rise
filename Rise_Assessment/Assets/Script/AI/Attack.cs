using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : AIBehaviour
{
    float attackTimer; // how long the attack actually lasts

    // Start is called before the first frame update
    void Start()
    {
        AI.ChangeAIAnimState(2);
        attackTimer = 0.25f;
    }

    public override void DoBehaviour()
    {
        if (attackTimer <= 0.0f)
        {
            AI.NewTopState(new AttackIdle());
        }
    }
}
