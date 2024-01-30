using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : INode
{
    private Func<bool> condition;
    //private Func<T, T, bool> condition;
    //T left;
    //T right;

    public ConditionNode(Func<bool> condition)
    {
        this.condition = condition;
    }

    public NodeState Execute()
    {
        return condition() ? NodeState.Success : NodeState.Failure;
    }

    //public ConditionNode(Func<T, T, bool> condition, T left, T right )
    //{
    //    this.condition = condition;
    //    this.left = left;
    //    this.right = right;
    //}

    //public NodeState Execute()
    //{
    //    return condition(left, right) ? NodeState.Success : NodeState.Failure;
    //}

    public void OnAwake()
    {
    }

    public void OnEnd()
    {
    }

    public void OnStart()
    {
    }
}
