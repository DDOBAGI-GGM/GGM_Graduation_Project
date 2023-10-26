using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_State : MonoBehaviour
{
    FSM_Brain _brain;
    FSM_Action _action;
    FSM_Decision _decision;

    private void Awake()
    {
        _brain = GetComponent<FSM_Brain>();
        _action = GetComponent<FSM_Action>();
        _decision = GetComponent<FSM_Decision>();
    }

    public void CheckTransition()
    {
        if (_decision.Decision())
            _brain.ChangeState(this);
    }

    public void PlayAction()
    {
        // BT »£√‚
    }
}
