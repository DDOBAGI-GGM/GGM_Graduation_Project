using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InverterNode : INode
{
    private INode node;

    public InverterNode(INode node)
    {
        this.node = node;
    }

    public void OnAwake()
    {
    }

    public void OnStart()
    {
    }

    public NodeState Execute()
    {
        NodeState result = node.Execute();
        if (result == NodeState.Success)
        {
            return NodeState.Failure;
        }
        else if (result == NodeState.Failure)
        {
            return NodeState.Success;
        }
        return result;
    }

    public void OnEnd()
    {
    }
}
