using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_PickAction : FSM_Action
{
    BT_Node _topNode;

    protected override void Awake()
    {
        base.Awake();

        BT_Exception _exception = new BT_Exception(ActionType.PICK, _brain._curObjType, DvcType.SOURCE);
        BT_PickDecision _decision = new BT_PickDecision();
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
