using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    private Stack<AIBehaviour> AIBehaviours;
    private int behaviourCount;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        AIBehaviours = new Stack<AIBehaviour>();
        AddState(new Idle()); //Idle is base state
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<PlayerController>().GetPlayerDead()) DoWork(); //if player not dead, do work
    }

    public void DoWork()
    {
        //AIBehaviours[behaviourCount - 1].DoBehaviour();
        AIBehaviours.Peek().DoBehaviour();
    }

    public void AddState(AIBehaviour behaviour)
    {
        AIBehaviours.Push(behaviour);
        behaviourCount = AIBehaviours.Count;
        AIBehaviours.Peek().Setup(GetComponent<AIController>());
    }

    public void PopState() // always remove the topstate
    {
        if (behaviourCount > 1)
        {
            AIBehaviours.Pop(); // Must always have at least idle in list
            behaviourCount = AIBehaviours.Count;
            if (behaviourCount == 1)
            {
                AIBehaviours.Peek().ResetTimer(); // We can reset here because we know it is the idle state
            }
        }
    }

    public void NewTopState(AIBehaviour behaviour)
    {
        if (behaviourCount > 1) // makes sure that always at least one state (idle) is in list
        {
            PopState();
        }
        AddState(behaviour);
    }

    public AIBehaviour GetTopState()
    {
        return AIBehaviours.Peek();
    }
}