using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStateNode : INode
{
    AI ai;
    AIStateType state;

    public CheckStateNode(AI ai, AIStateType state)
    {
        this.ai = ai;
        this.state = state;
    }

    public void OnAwake()
    {
    }

    public void OnStart()
    {
    }

    public NodeState Execute()
    {
        if (ai.stateType == state)
            return NodeState.Success;
        return NodeState.Failure;
    }

    public void OnEnd()
    {
    }
}
