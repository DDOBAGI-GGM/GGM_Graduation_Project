using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

public class FSM_Brain : MonoBehaviour
{
    NavMeshAgent _agent;

    [SerializeField] FSM_State curState;
    public List<GameObject> targets = new List<GameObject>();
    [SerializeField] List<FSM_State> states = new List<FSM_State>();

    private void Awake()
    {
        _agent= GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        foreach (FSM_State state in states)
            state.CheckTransition();

        curState?.PlayAction();
    }

    public void ChangeState(FSM_State _state)
    {
        curState = _state;
    }
}
