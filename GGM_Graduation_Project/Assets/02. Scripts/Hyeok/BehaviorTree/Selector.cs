using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Selector : Node
    {
        protected List<Node> _nodes = new List<Node>();

        public Selector(List<Node> nodes)
        {
            _nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            foreach (var n in _nodes)
            {
                switch (n.Evaluate())
                {
                    case NodeState.RUNNING:
                        _nodeState = NodeState.RUNNING;
                        return _nodeState;
                    case NodeState.SUCCESS:
                        _nodeState = NodeState.SUCCESS;
                        return _nodeState;
                    case NodeState.FAILURE:
                        break;
                }
            }

            //여기까지 와 있다는건 다 실패
            _nodeState = NodeState.FAILURE;
            return _nodeState;
        }
    }
}
