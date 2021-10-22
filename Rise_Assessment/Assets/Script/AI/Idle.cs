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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DoBehaviour()
    {
        if (waitTimer <= 0.0f)
        {
            AI.RemoveState(); // Removes Idle to go back to patrol
        }
        waitTimer -= Time.deltaTime;
    }
}
