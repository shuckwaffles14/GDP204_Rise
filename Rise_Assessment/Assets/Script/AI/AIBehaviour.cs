using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
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

    }

    public void CheckForPlayer()
    {
        RaycastHit2D hit;
        float vd = AI.GetViewDistance();

        if (AI.GetDirection()) // if AI looking right
        {
            //will keep it simple for now with single raycast forward, will update later to have a cone of vision
            hit = Physics2D.Raycast(AI.eyesRight.transform.position, Vector2.right, vd);
        }
        else // if AI looking left
        {
            hit = Physics2D.Raycast(AI.eyesRight.transform.position, Vector2.left, vd);
        }

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                //do stuff
            }
        }
    }
}
