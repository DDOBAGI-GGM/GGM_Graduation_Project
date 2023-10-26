using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public enum NodeType
    {
        SUCCESS = 1,
        FAILURE,
        RUNNING
    }

    public enum ActionType
    {
        NONE = 0,
        PICK,
        MAKING,
        ATTACK
    }

    public enum ObjectType
    {
        NONE = 0,
        SOURCE,
        PROCESSED,
        PRODUCT
    }

    public enum DeviceType
    {
        SOURCE = 0,
        PROCESS_FIRST,
        PROCESS_SECOND,
        ATTACK,
        BREAKDOWN
    }

    public abstract class BT_Node : MonoBehaviour
    {
        protected NodeType _nodeState;
        public NodeType nodeType => _nodeState;
        protected ActionType _action = ActionType.NONE;

        public abstract NodeType Evaluate();
    }
}
