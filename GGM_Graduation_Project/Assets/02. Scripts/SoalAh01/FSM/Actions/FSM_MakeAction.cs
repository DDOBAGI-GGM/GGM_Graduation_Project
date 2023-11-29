using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_MakeAction : FSM_Action
{
    public override void Action(GameObject _destination)
    {
        // 도착했다면
        if (_brain.SetDestination(_destination))
        {
            Debug.Log("조리 액션 실행중");
            _brain._hand = _brain.interaction_Dic[deviceName].Interaction(_brain._hand);
            _brain.Reset();
        }
    }
}
