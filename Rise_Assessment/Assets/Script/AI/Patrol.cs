using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : AIBehaviour
{
    private Vector2 target;
    private Vector2 currentPosition;
    private float objSensitivity;

    // Start is called before the first frame update
    void Start()
    {
        objSensitivity = AI.GetCheckpointSensitivity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DoBehaviour()
    {
        target = AI.GetCurrentTarget();
        currentPosition = AI.transform.position;
        if (Vector2.Distance(currentPosition, target) < objSensitivity) // if object is close enough to checkpoint
        {
            AI.AddState(new Idle());
        }
    }
}
