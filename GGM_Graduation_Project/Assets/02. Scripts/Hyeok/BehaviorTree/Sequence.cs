using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        protected List<Node> _nodes = new List<Node>();
        public Sequence(List<Node> nodes)
        {
            _nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            bool isAnyNodeRun = false;

            foreach (var n in _nodes)
            {
                switch (n.Evaluate())
                {
                    case NodeState.RUNNING:
                        isAnyNodeRun = true;
                        break;
                    case NodeState.SUCCESS:
                        break;
                    case NodeState.FAILURE:
                        _nodeState = NodeState.FAILURE;
                        return _nodeState;
                }
            }

            _nodeState = isAnyNodeRun ? NodeState.RUNNING : NodeState.SUCCESS;
            return _nodeState;
        }
    }
}
