using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace BehaviorTree
{
    public class BT_Exception : BT_Node
    {
        private ActionType actionType;
        private ObjectType objectType;
        private DvcType deviceType;
       
        public BT_Exception(ActionType _actType, ObjectType _objType, DvcType _dvcType)
        {
            actionType = _actType;
            objectType = _objType;
            deviceType = _dvcType;
        }

        public override NodeType Evaluate()
        {
            switch (actionType)
            {
                case ActionType.PICK:
                    return objectType == ObjectType.NONE ? NodeType.SUCCESS : NodeType.FAILURE;

                case ActionType.MAKING:
                    if (objectType == ObjectType.NONE)
                        return NodeType.FAILURE;
                    else if (deviceType == DvcType.BREAKDOWN)
                        return NodeType.FAILURE;
                    else if ((int)deviceType != (int)objectType)
                        return NodeType.FAILURE;
                    return NodeType.SUCCESS;

                case ActionType.ATTACK:
                    if (objectType != ObjectType.PRODUCT)
                        return NodeType.FAILURE;
                    return NodeType.SUCCESS;
            }

            return NodeType.FAILURE;
        }
    }
}
