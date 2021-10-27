using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    public class Check
    {
        public bool playerSighted;
        public Vector2 targetLocation;
    }

    public AIController AI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void DoBehaviour()
    {
        GetPositiveCheck(CheckForPlayer()); // if player is found swap behaviour to either attack or move to attack
    }

    public Check CheckForPlayer()
    {
        RaycastHit2D hit;
        Check thisCheck = new Check();
        float vd = AI.GetViewDistance();

        if (AI.GetDirection()) // if AI looking right
        {
            //will keep it simple for now with single raycast forward, will update later to have a cone of vision
            hit = Physics2D.Raycast(AI.eyesRight.transform.position, Vector2.right, vd);
        }
        else // if AI looking left
        {
            hit = Physics2D.Raycast(AI.eyesLeft.transform.position, Vector2.left, vd);
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

        return null;
    }

    public void GetPositiveCheck(Check _check) // works with CheckForPlayer() so if player is found, AI knows to change behaviour
    {
        if (_check != null) // if check came up with a result
        {
            if (Vector2.Distance(_check.targetLocation, AI.transform.position) <= AI.attackRange) //if within attack range
            {
                AI.NewTopState(new Attack());
            }
            AI.NewTopState(new MoveToAttack());
        }
        else // if check came up with no results
        {
            AI.RemoveState();// go back to idle, then patrol.
        }
    }
}
