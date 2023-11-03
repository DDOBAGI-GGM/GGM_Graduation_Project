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
        // // while�� ������ ������ �� �� �˻��� �� �̵����� ��ų���� �ƴ� �Ÿ� �� �Ǹ� fsm�� false ��ȯ�Ұ���?
        //}

        return _brain.SetDestination(_dvcType) ? NodeType.SUCCESS : NodeType.FAILURE;
    }
}
