using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_AttackAction : FSM_Action
{
    public override void Action()
    {
        // 한 번 액션을 실행 성공하면 state none으로 바꾸기
        _brain.ChangeState(null);
    }
}
