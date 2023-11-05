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
        ATTACK,
        FIX,
        THROWOUT
    }

    public enum ObjectType
    {
        NONE = 0,
        SOURCE,
        PROCESSED,
        PRODUCT
    }

    public enum DvcType
    {
        SOURCE = 0,
        PROCESS_FIRST,
        PROCESS_SECOND,
        ATTACK,
        GARBAGE,
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
