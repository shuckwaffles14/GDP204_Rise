using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    private List<AIBehaviour> AIBehaviours;

    public AIStateMachine(AIController _AI)
    {
        AIBehaviours.Add(new Idle()); //Idle is base state
        AIBehaviours[0].AI = _AI;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoWork()
    {
        int count = AIBehaviours.Count;
        AIBehaviours[count - 1].DoBehaviour();
    }

    public void AddState(AIBehaviour behaviour)
    {
        AIBehaviours.Add(behaviour);
    }

    public void PopState() // always remove the topstate
    {
        if (AIBehaviours.Count > 1) AIBehaviours.RemoveAt(AIBehaviours.Count); // Must always have at least idle in list
    }

    public void NewTopState(AIBehaviour behaviour)
    {
        int count = AIBehaviours.Count;
        if (count > 1) // makes sure that always at least one state (idle) is in list
        {
            PopState();
        }
        AddState(behaviour);
    }
}
