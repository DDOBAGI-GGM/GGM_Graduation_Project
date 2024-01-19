using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : INode
{
    private Action action;

    public ActionNode(Action action)
    {
        this.action = action;
    }

    public NodeState Execute()
    {
        action();
        return NodeState.Success;
    }

    public void OnAwake()
    {
        throw new NotImplementedException();
    }

    public void OnEnd()
    {
        throw new NotImplementedException();
    }

    public void OnStart()
    {
        throw new NotImplementedException();
    }
}