using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FSM_Decision : MonoBehaviour
{
    protected FSM_Brain _brain;

    BT_Node _topNode;
    protected BT_Decision _decision;
    protected BT_Exception _exception;

    protected void Awake()
    {
        _brain = GetComponentInParent<FSM_Brain>();

        _decision = GetComponent<BT_Decision>();
        _exception = GetComponentInParent<BT_Exception>();
        _topNode = new BT_Sequence(new List<BT_Node> { _exception, _decision });
    }

    public bool Decision()
    {
        if (_topNode.Evaluate() == NodeType.SUCCESS)
            return true;
        return false;
    }
}
