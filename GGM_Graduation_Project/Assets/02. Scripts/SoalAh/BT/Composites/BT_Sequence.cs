using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class BT_Sequence : BT_Node
    {
        protected List<BT_Node> nodes = new List<BT_Node>();
        public BT_Sequence(List<BT_Node> _nodes) { nodes = _nodes; }

        public override NodeType Evaluate()
        {
            foreach (BT_Node node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeType.RUNNING:
                        _nodeState = NodeType.RUNNING;
                        break;
                    case NodeType.SUCCESS:
                        _nodeState = NodeType.SUCCESS;
                        break;
                    case NodeType.FAILURE:
                        _nodeState= NodeType.FAILURE;
                        return _nodeState;
                }
            }

            return _nodeState;
        }
    }
}
