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

    public TNodeState Execute()
    {
        foreach (var node in nodes)
        {
            if (node.Execute() == TNodeState.Failure)
            {
                return TNodeState.Failure;
            }
        }
        return TNodeState.Success;
    }
}