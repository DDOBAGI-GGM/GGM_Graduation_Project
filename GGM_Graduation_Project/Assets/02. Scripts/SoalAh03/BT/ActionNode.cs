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
    public TNodeState Execute()
    {
        action();
        return TNodeState.Success;
    }
}