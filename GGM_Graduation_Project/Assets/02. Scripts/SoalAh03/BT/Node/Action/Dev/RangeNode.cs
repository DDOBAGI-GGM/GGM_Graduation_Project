using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : INode
{
    AI ai;
    public RangeNode(AI ai)
    {
        this.ai = ai;
    }

    public void OnAwake()
    {
    }

    public void OnStart()
    {
    }

    public NodeState Execute()
    {
        if (ai.destination == null)
            return NodeState.Failure;

        float distance = Vector3.Distance(ai.transform.position , ai.destination.transform.position);
        if (distance < 2f)
            return NodeState.Success;
        return NodeState.Failure;
    }

    public void OnEnd()
    {
    }
}
