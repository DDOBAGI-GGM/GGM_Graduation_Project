using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_MakeAction : FSM_Action
{
    BT_Node _topNode;

    protected override void Awake()
    {
        base.Awake();

        DvcType _dvcType = (DvcType)(int)_brain._curObjType;

        BT_Exception _exception = new BT_Exception(ActionType.MAKING, _brain._curObjType, _dvcType);
        BT_MakeDecision _decision = new BT_MakeDecision();
        _topNode = new BT_Sequence(new List<BT_Node> { _exception, _decision });
    }

    public override void Action()
    {
        if (_topNode.Evaluate() == NodeType.SUCCESS)
        {
            // ¾×¼Ç
        }
    }
}
