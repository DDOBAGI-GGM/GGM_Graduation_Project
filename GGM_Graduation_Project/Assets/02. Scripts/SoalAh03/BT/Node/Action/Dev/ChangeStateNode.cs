using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStateNode : INode
{
    AI ai;
    AIStateType state;

    public ChangeStateNode(AI ai, AIStateType state)
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
        ai.stateType = state;
        return NodeState.Success;
    }

    public void OnEnd()
    {
    }
}
