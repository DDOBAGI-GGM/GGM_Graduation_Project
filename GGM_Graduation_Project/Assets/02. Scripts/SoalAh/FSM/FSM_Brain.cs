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
    //public List<GameObject> targets = new List<GameObject>(); // 오브젝트 리스트

    [SerializeField] GameObject _hand = null;
    public ObjectType _curObjType = ObjectType.NONE;

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
    }

    public void ChangeState(FSM_State _state)
    {
        // 상태 변환
        curState = _state;
    }

    public bool SetDestination(List<GameObject> _targets, float _speed)
    {
        // 속도 설정
        _agent.speed = _speed;
        // 목적지로 이동
        // 도마가 2개일 수도 있잖아... 그럼 두 개 중에 어떤 도마를 쓸건지? 확인하는? for
        //_agent.SetDestination(_target.transform.position);

        // 플레이어가 목적지에 거의 다 도달했다면
        if (_agent.remainingDistance > 0.5f && _agent.velocity.sqrMagnitude > 0.5f)
            return true;

        return false;
    }
}
