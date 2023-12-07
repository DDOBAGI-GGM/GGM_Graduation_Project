using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode : INode
{
    private Func<bool> action;

    public MoveNode(Func<bool> action)
    {
        this.action = action;
    }
    public TNodeState Execute()
    {
        while (action())
            action();
        return TNodeState.Success;
    }
}
