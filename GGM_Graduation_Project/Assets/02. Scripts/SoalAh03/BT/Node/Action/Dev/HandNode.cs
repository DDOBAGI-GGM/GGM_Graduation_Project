using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text.RegularExpressions;

public class HandNode : INode
{
    AI ai;

    public HandNode(AI ai)
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
        return NodeState.Success;
    }

    public void OnEnd()
    {
    }
}
