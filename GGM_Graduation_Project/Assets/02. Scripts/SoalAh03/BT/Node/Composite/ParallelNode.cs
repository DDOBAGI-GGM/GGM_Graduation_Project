using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ParallelNode : INode
{
    private List<INode> nodes;

    public ParallelNode(params INode[] nodes)
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
        bool complete = true;
        foreach (var node in nodes)
        {
            NodeState result = node.Execute();
            switch (result)
            {
                case NodeState.Failure:
                    node.OnEnd();
                    node.OnStart();
                    continue;
                case NodeState.Running:
                    return NodeState.Running;
            }
        }
        return complete ? NodeState.Success : NodeState.Running;
    }

    public void OnEnd()
    {
        foreach (var node in nodes)
        {
            node.OnEnd();
        }
    }
}
