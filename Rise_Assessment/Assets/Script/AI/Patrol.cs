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
        target = AI.GetCurrentTarget();
    }

    public override void DoBehaviour()
    {
        GetPositiveCheck(CheckForPlayer()); // if player is found swap behaviour to either attack or move to attack

        currentPosition = AI.transform.position;
        if (Vector2.Distance(currentPosition, target) < objSensitivity) // if object is close enough to checkpoint
        {
            AI.GetNextCheckpoint(); // gets the next checkpoint ready for the next patrol state
            AI.RemoveState(); // go back to idle
        }
    }
}
