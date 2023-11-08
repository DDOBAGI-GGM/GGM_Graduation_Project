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

    [Header("State")]
    [SerializeField] FSM_State curState; // ���� ����
    [SerializeField] List<FSM_State> states = new List<FSM_State>(); // ���� ����Ʈ
    public List<GameObject> objPos = new List<GameObject>(); // ������Ʈ ����Ʈ

    [Header("Hand")]
    [SerializeField] GameObject _hand = null; // ���� AI�� ��� �ִ� ������
    public ObjectType _curObjType = ObjectType.NONE; // �տ� ����ִ� ������ Ÿ��

    protected GameObject destination;

    private void Awake()
    {
        _agent= GetComponent<NavMeshAgent>();
    }

    public void Reset()
    {
        curState = null;
        destination = null;
    }

    private void Update()
    {
        // ���� �˻�
        foreach (FSM_State state in states)
            state.CheckTransition();

        // �ൿ ����
        curState?.PlayAction(destination);

        // �� ����...
        hand();
    }

    public void ChangeState(FSM_State _state)
    {
        // ���� ��ȯ
        curState = _state;
    }

    public bool SetDestination(GameObject _targets)
    {
        // ������ ������ �ݺ�
        while (true)
        {
            // �����ߴٸ�
            if (_agent.remainingDistance > 0.5f && _agent.velocity.sqrMagnitude > 0.5f) 
                return true;
        }
    }

    private void hand()
    {
        // �տ� � Ÿ���� ������Ʈ�� �ִ���

        // �տ� �ƹ��͵� ������� �ʴٸ� none
        if (_hand.transform.childCount == 0)
            _curObjType = ObjectType.NONE;
        // �տ� ���𰡸� ����ִٸ� �� ������Ʈ�� recipe�� ���� ������Ʈ Ÿ�� �޾ƿ���
        else
            _curObjType = _hand.transform.GetChild(0).GetComponent<RecipeSO>().objType;
    }
}
