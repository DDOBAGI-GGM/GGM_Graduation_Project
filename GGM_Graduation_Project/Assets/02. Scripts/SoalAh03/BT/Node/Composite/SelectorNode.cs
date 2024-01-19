using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : INode
{
    private List<INode> nodes;

    public SelectorNode(params INode[] nodes)
    {
        this.nodes.AddRange(nodes);
    }

    public void OnAwake()
    {
        nodes = new List<INode>();

        foreach (var node in nodes)
        {
            node.OnAwake();
            node.OnStart();
        }
    }

    public void OnStart()
    {
        foreach (var node in nodes)
        {
            node.OnStart();
        }
    }

    public NodeState Execute()
    {
        foreach (var node in nodes)
        {
            NodeState result = node.Execute();
            switch (result)
            {
                case NodeState.Success:
                    node.OnEnd();
                    node.OnStart();
                    continue;
                case NodeState.Failure:
                    node.OnEnd();
                    node.OnStart();
                    continue;
                case NodeState.Running:
                    return NodeState.Running;
            }
            //node.OnStart();
        }
        return NodeState.Failure;
    }

    public void OnEnd()
    {
        foreach (var node in nodes)
        {
            node.OnEnd();
        }
    }
}
