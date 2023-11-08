using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_AttackDecision : BT_Decision
{
    public override NodeType Evaluate()
    {
        return NodeType.SUCCESS;
    }
}
