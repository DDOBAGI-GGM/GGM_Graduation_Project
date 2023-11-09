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
        _brain = GetComponentInParent<FSM_Brain>();
        _action = GetComponent<FSM_Action>();
        _decision = GetComponent<FSM_Decision>();
    }

    public void CheckTransition()
    {
        if (_decision.Decision()) // ���¸� ������ �� �ִ��� Ȯ��
            _brain.ChangeState(this); // ���¸� ������ �� �ִٸ� ���� ���¸� �ѱ��
    }

    public void PlayAction(GameObject _destination)
    {
        _action.Action(_destination); // �׼� ����
    }
}
