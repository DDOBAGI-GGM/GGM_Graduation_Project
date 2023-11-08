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

    [SerializeField] FSM_State curState; // 현재 상태
    [SerializeField] List<FSM_State> states = new List<FSM_State>(); // 상태 리스트
    public List<GameObject> objPos = new List<GameObject>(); // 오브젝트 리스트

    [SerializeField] GameObject _hand = null; // 현재 AI가 들고 있는 아이템
    public ObjectType _curObjType = ObjectType.NONE; // 손에 들려있는 아이템 타입

    private void Awake()
    {
        _agent= GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // 상태 검사
        foreach (FSM_State state in states)
            state.CheckTransition();

        // 행동 실행
        curState?.PlayAction();

        hand();
        // 손 상태...
    }

    public void ChangeState(FSM_State _state)
    {
        // 상태 변환
        curState = _state;
    }

    //public bool SetDestination(DvcType _targets)
    public bool SetDestination(GameObject _targets)
    {
        // 속도 설정
        //_agent.speed = _speed;
        // 목적지로 이동 (1. 종류, 2. 사용에 가능?, 3. 가장 가까운)

        // 도마가 2개일 수도 있잖아... 그럼 두 개 중에 어떤 도마를 쓸건지? 확인하는? for
        //_agent.SetDestination(objPos[m].transform.position);

        // 플레이어가 목적지에 거의 다 도달했다면
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
