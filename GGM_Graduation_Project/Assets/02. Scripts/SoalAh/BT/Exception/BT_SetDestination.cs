using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_SetDestination : BT_Node
{
    FSM_Brain _brain;
    DvcType _dvcType;

    public BT_SetDestination(FSM_Brain _brain, DvcType _dvcType)
    {
        this._brain = _brain;
        this._dvcType = _dvcType;
    }

    public override NodeType Evaluate()
    {
        //while (_brain.SetDestination(_dvcType) == false)
        //{
        // // while로 돌려서 무조건 한 번 검사할 때 이동까지 시킬건지 아님 거리 안 되면 fsm에 false 반환할건지?
        //}

        return _brain.SetDestination(_dvcType) ? NodeType.SUCCESS : NodeType.FAILURE;
    }
}
