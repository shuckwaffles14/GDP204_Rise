using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : AIBehaviour
{
    public override void Setup(AIController _AIController)
    {
        AI = _AIController;
        AI.ChangeAIAnimState(3);
    }

    public override void DoBehaviour()
    {
        //do nothing because dead
    }
}
