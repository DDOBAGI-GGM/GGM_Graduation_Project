using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_MakeAction : FSM_Action
{
    public override void Action()
    {
        Debug.Log("선택 액션 실행중");
    }
}
