using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FSM_Brain : MonoBehaviour
{
    NavMeshAgent _agent;

    [SerializeField] FSM_State curState; // ���� ����
    [SerializeField] List<FSM_State> states = new List<FSM_State>(); // ���� ����Ʈ
    //public List<GameObject> targets = new List<GameObject>(); // ������Ʈ ����Ʈ

    [SerializeField] GameObject _hand = null;
    public ObjectType _curObjType = ObjectType.NONE;

    private void Awake()
    {
        _agent= GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // ���� �˻�
        foreach (FSM_State state in states)
            state.CheckTransition();

        // �ൿ ����
        curState?.PlayAction();
    }

    public void ChangeState(FSM_State _state)
    {
        // ���� ��ȯ
        curState = _state;
    }

    public bool SetDestination(List<GameObject> _targets, float _speed)
    {
        // �ӵ� ����
        _agent.speed = _speed;
        // �������� �̵�
        // ������ 2���� ���� ���ݾ�... �׷� �� �� �߿� � ������ ������? Ȯ���ϴ�? for
        //_agent.SetDestination(_target.transform.position);

        // �÷��̾ �������� ���� �� �����ߴٸ�
        if (_agent.remainingDistance > 0.5f && _agent.velocity.sqrMagnitude > 0.5f)
            return true;

        return false;
    }
}
