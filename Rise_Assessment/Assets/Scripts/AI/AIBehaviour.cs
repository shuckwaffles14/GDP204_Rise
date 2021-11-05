using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour
{
    public string behaviourName;
    public class Check
    {
        public bool playerSighted;
        public Vector2 targetLocation;
    }

    public AIController AI;

    public virtual void Setup(AIController _AIController)
    {
        // do nothing in this level
    }

    public virtual void DoBehaviour()
    {
        GetPositiveCheck(CheckForPlayer()); // if player is found swap behaviour to either attack or move to attack
    }

    public virtual void ResetTimer()
    {
        // do nothing at this level
    }

    public Check CheckForPlayer()
    {
        RaycastHit2D hit;
        Check thisCheck = new Check();
        float vd = AI.GetViewDistance();
        bool dir = AI.GetDirection();

        for (int iterator = 0; iterator < 5; iterator++)
        {
            if (dir) // if AI looking right
            {
                Vector2 rayDir = Vector2.right;
                rayDir.y += GetRays(iterator);
                //will keep it simple for now with single raycast forward, will update later to have a cone of vision
                hit = Physics2D.Raycast(AI.eyesRight.transform.position, rayDir, vd);
            }
            else // if AI looking left
            {
                Vector2 rayDir = Vector2.left;
                rayDir.y += GetRays(iterator);
                hit = Physics2D.Raycast(AI.eyesLeft.transform.position, rayDir, vd);
            }

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    thisCheck.playerSighted = true;
                    thisCheck.targetLocation.x = hit.transform.position.x;
                    thisCheck.targetLocation.y = hit.transform.position.y;
                    AI.UpdateTarget(thisCheck.targetLocation);
                    return thisCheck;
                }
            }
        }

        return null;
    }

    public void GetPositiveCheck(Check _check) // works with CheckForPlayer() so if player is found, AI knows to change behaviour
    {
        AIBehaviour currentBehaviour = AI.GetTopState();
        if (_check != null) // if check came up with a result
        {
            if (Vector2.Distance(_check.targetLocation, AI.transform.position) <= AI.attackRange) //if within attack range
            {
                // Don't need to check current behaviour, AI will not do this func in Attack() behaviour
                AI.UpdateTarget(_check.targetLocation);
                AI.NewTopState(new Attack());
            }
            if (currentBehaviour.behaviourName != "MoveToAttack") // Only move to attack if not already moving to attack
            {
                AI.NewTopState(new MoveToAttack());
            }
        }
        else // if no result from check
        {
            if (currentBehaviour.behaviourName == "Idle") // if AI in Idle, do nothing
            {
                return;
            }

            if (currentBehaviour.behaviourName != "Patrol") // Only patrol if not already patrolling
            {
                AI.NewTopState(new Patrol());
            }
        }
    }

    float GetRays(int _iterator)
    {
        if (_iterator == 0)
        {
            return 1f;
        }
        else if (_iterator == 1)
        {
            return 0.5f;
        }
        else if (_iterator == 2)
        {
            return 0f;
        }
        else if (_iterator == 3)
        {
            return -0.5f;
        }
        else if (_iterator == 4)
        {
            return -1f;
        }

        return 0f;
    }
}