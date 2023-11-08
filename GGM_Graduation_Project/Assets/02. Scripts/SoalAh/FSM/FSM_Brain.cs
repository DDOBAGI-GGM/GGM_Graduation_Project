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
    [SerializeField] FSM_State curState; // 현재 상태
    [SerializeField] List<FSM_State> states = new List<FSM_State>(); // 상태 리스트
    public List<GameObject> objPos = new List<GameObject>(); // 오브젝트 리스트

    [Header("Hand")]
    [SerializeField] GameObject _hand = null; // 현재 AI가 들고 있는 아이템
    public ObjectType _curObjType = ObjectType.NONE; // 손에 들려있는 아이템 타입

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
        // 상태 검사
        foreach (FSM_State state in states)
            state.CheckTransition();

        // 행동 실행
        curState?.PlayAction(destination);

        // 손 상태...
        hand();
    }

    public void ChangeState(FSM_State _state)
    {
        // 상태 변환
        curState = _state;
    }

    public bool SetDestination(GameObject _targets)
    {
        // 도착할 때까지 반복
        while (true)
        {
            // 도착했다면
            if (_agent.remainingDistance > 0.5f && _agent.velocity.sqrMagnitude > 0.5f) 
                return true;
        }
    }

    private void hand()
    {
        // 손에 어떤 타입의 오브젝트가 있는지

        // 손에 아무것도 들고있지 않다면 none
        if (_hand.transform.childCount == 0)
            _curObjType = ObjectType.NONE;
        // 손에 무언가를 들고있다면 손 오브젝트의 recipe를 통해 오브젝트 타입 받아오기
        else
            _curObjType = _hand.transform.GetChild(0).GetComponent<RecipeSO>().objType;
    }
}
