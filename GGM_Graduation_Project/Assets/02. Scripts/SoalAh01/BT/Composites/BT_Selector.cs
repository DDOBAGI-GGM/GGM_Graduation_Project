using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Selector : BT_Node
{
    protected List<BT_Node> nodes = new List<BT_Node>();
    public BT_Selector(List<BT_Node> _nodes) { nodes = _nodes; }

    public override NodeType Evaluate()
    {
        foreach (BT_Node node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeType.RUNNING:
                    _nodeState = NodeType.RUNNING;
                    return _nodeState;
                case NodeType.SUCCESS:
                    _nodeState = NodeType.SUCCESS;
                    return _nodeState;
                case NodeType.FAILURE:
                    break;
            }
        }

        _nodeState = NodeType.FAILURE;
        return _nodeState;
    }
}
