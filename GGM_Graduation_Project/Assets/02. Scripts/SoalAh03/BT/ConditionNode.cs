using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : INode
{
    private Func<bool> condition;

    public ConditionNode(Func<bool> condition)
    {
        this.condition = condition;
    }

    public TNodeState Execute()
    {
        return condition() ? TNodeState.Success : TNodeState.Failure;
    }
}
