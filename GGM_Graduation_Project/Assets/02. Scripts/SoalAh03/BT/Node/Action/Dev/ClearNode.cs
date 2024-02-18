using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearNode<T> :INode
{
    AI ai;
    T obj;
 
    public ClearNode(AI ai, ref T obj)
    {
        this.ai = ai;
        this.obj = obj;
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
