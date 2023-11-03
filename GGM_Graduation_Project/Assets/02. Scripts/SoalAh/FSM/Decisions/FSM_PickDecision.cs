using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_PickDecision : FSM_Decision
{
    BT_Node _topNode;

    protected override void Awake()
    {
        base.Awake();

        BT_Exception _exception = new BT_Exception(ActionType.PICK, _brain._curObjType, DvcType.SOURCE);
        BT_PickDecision _decision = new BT_PickDecision();
        BT_SetDestination _setDestination = new BT_SetDestination(_brain, DvcType.SOURCE);
        _topNode = new BT_Sequence(new List<BT_Node> { _exception, _decision, _setDestination });
    }

    public override bool Decision()
    {
        if (_topNode.Evaluate() == NodeType.SUCCESS)
            return true;
        return false;
    }
}
