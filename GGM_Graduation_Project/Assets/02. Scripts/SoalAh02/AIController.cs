using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    //private FSM<IState> stateManager;
    //private BehaviourTreeManager<INode> btManager;

    //private NavMeshAgent agent;

    //private Transform destination;

    //private void Awake()
    //{
    //    agent = GetComponent<NavMeshAgent>();
    //}

    //private void Start()
    //{
    //    stateManager = new FSM<IState>();
    //    stateManager.AddState("Idle", new IdleState());
    //    stateManager.AddState("Move", new MoveState());
    //    stateManager.ChangeState("Idle");

    //    btManager = new BehaviourTreeManager<INode>();
    //    btManager.SetRoot(new SelectorNode(new SequenceNode(
    //        new ConditionNode(CanMove),
    //        new ActionNode(Move))));
    //}

    //private void Update()
    //{
    //    stateManager.Update();
    //    btManager.Update();
    //}

    //bool CanMove()
    //{
    //    return true;
    //}
    //void Move()
    //{
    //    Debug.Log("�̵���");
    //}
}

// �Ÿ� üũ (nav, destination)
// �� üũ (hand, type?)
// ���� üũ (destination�� is�ӽñ�...)
// �׼� (����)
