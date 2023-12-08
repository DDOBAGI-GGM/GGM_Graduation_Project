using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : INode
{
    private List<INode> nodes = new List<INode>();

    public SelectorNode(params INode[] nodes)
    {
        this.nodes.AddRange(nodes);
    }

    public TNodeState Execute()
    {
        foreach (var node in nodes)
        {
            if (node.Execute() == TNodeState.Success)
            {
                return TNodeState.Success;
            }
        }
        return TNodeState.Failure;
    }
}
