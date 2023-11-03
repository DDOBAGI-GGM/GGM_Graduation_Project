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
        if (_decision.Decision()) // 상태를 변경할 수 있는지 확인
            _brain.ChangeState(this); // 상태를 변경할 수 있다면 현재 상태를 넘긴다
    }

    public void PlayAction()
    {
        _action.Action(); // 액션 실행
    }
}
