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
    public List<GameObject> objPos = new List<GameObject>(); // ������Ʈ ����Ʈ

    [SerializeField] GameObject _hand = null; // ���� AI�� ��� �ִ� ������
    public ObjectType _curObjType = ObjectType.NONE; // �տ� ����ִ� ������ Ÿ��

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

        hand();
        // �� ����...
    }

    public void ChangeState(FSM_State _state)
    {
        // ���� ��ȯ
        curState = _state;
    }

    //public bool SetDestination(DvcType _targets)
    public bool SetDestination(GameObject _targets)
    {
        // �ӵ� ����
        //_agent.speed = _speed;
        // �������� �̵� (1. ����, 2. ��뿡 ����?, 3. ���� �����)

        // ������ 2���� ���� ���ݾ�... �׷� �� �� �߿� � ������ ������? Ȯ���ϴ�? for
        //_agent.SetDestination(objPos[m].transform.position);

        // �÷��̾ �������� ���� �� �����ߴٸ�
        if (_agent.remainingDistance > 0.5f && _agent.velocity.sqrMagnitude > 0.5f)
            return true;

        return false;
    }

    private void hand()
    {
        if (_hand.transform.childCount == 0)
            _curObjType = ObjectType.NONE;
        else
            _curObjType = _hand.transform.GetChild(0).GetComponent<RecipeSO>().objType;
    }
 }
