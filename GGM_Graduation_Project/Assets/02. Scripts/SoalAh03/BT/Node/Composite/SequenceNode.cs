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

    public void OnAwake()
    {
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
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Failure:
                    node.OnEnd();
                    node.OnStart();
                    return NodeState.Failure;
            }
            //node.OnStart();
        }
        return NodeState.Success;
    }

    public void OnEnd()
    {
        foreach (var node in nodes)
        {
            node.OnEnd();
        }
    }

    //public NodeState Execute()
    //{
    //    foreach (var node in nodes)
    //    {
    //        if (node.Execute() == NodeState.Failure)
    //        {
    //            return NodeState.Failure;
    //        }
    //    }
    //    return NodeState.Success;
    //}
}
