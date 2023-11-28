using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : INode
{
    private List<INode> nodes = new List<INode>();

    public SequenceNode(params INode[] nodes)
    {
        this.nodes.AddRange(nodes);
    }

    public NodeState Execute()
    {
        foreach (var node in nodes)
        {
            if (node.Execute() == NodeState.Failure)
            {
                return NodeState.Failure;
            }
        }
        return NodeState.Success;
    }
}