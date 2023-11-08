using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Decision : BT_Node
{
    public override NodeType Evaluate()
    {
        Debug.Log("중간 값 실행...");
        throw new System.NotImplementedException();
    }
}
