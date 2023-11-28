using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private FSM<stateType> stateManager;
    //private BehaviourTreeManager<INode> behaviourTreeManager;

    //void Start()
    //{
    //     //State Manager 초기화
    //    stateManager = new FSM<stateType>();
    //    stateManager.AddState("Move", new MoveState());
    //     //다른 상태들 추가

    //     //Behaviour Tree Manager 초기화
    //    behaviourTreeManager = new BehaviourTreeManager<INode>();
    //    behaviourTreeManager.SetRoot(new SequenceNode(
    //        new SelectorNode(
    //            new ConditionNode(CanMove),
    //            new ActionNode(MoveAction)
    //        ),
    //         //다른 노드들 추가
    //    ));
    //}

    //void Update()
    //{
    //     //State Manager 및 Behaviour Tree Manager 업데이트
    //    stateManager.Update();
    //    behaviourTreeManager.Update();
    
    ////조건 및 액션 메서드들 구현
    //bool CanMove() { }
    //void MoveAction() { }
    ////다른 메서드들 구현

}
