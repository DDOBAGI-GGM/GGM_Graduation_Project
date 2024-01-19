using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeaterNode : INode
{
    private INode node;
    private bool forever;
    private int count;

    private int currentCount;

    public RepeaterNode(INode node, bool forever, int count = 1)
    {
        this.node = node;
    }

    public void OnAwake()
    {
    }

    public void OnStart()
    {
        currentCount = count;
    }

    public NodeState Execute()
    {
        // 수정 필요함, 사용하지 말 것
        if (forever)
        {
            while (forever)
            {
                NodeState result = node.Execute();
                if (result == NodeState.Failure)
                    return NodeState.Failure;
            }
        }
        else
        {
            while (currentCount <= count)
            {
                NodeState result = node.Execute();
                if (result == NodeState.Failure)
                    return NodeState.Failure;
                currentCount++;
            }
        }
        
        return NodeState.Success;
    }

    public void OnEnd()
    {
    }
}
