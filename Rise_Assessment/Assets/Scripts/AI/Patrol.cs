using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : AIBehaviour
{
    private Vector2 target; // in this context, checkpoint location
    private Vector2 currentPosition;
    private float objSensitivity;
    //private AIController ourAI; // reference to the AI the owns the behaviour

    public override void Setup(AIController _AIController)
    {
        AI = _AIController;
        objSensitivity = AI.GetCheckpointSensitivity();
        target = AI.GetCurrentTarget();
        AI.ChangeAIAnimState(1);
        behaviourName = "Patrol";
    }

    public override void DoBehaviour()
    {
        base.DoBehaviour(); // check for player
        Debug.Log("Patrol");
        currentPosition = AI.transform.position;

        if (currentPosition.x >= target.x) // if to right of target
        {
            AI.Move(false); // move left
        }
        else // if to left of target
        {
            AI.Move(true); // move right
        }
        AI.ChangeAIAnimState(1);

        if (Vector2.Distance(currentPosition, target) <= objSensitivity) // if object is close enough to checkpoint
        {
            AI.GetNextCheckpoint(); // gets the next checkpoint ready for the next patrol state
            AI.RemoveState(); // go back to idle
        }
    }
}