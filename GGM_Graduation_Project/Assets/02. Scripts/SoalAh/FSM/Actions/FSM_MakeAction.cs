using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_MakeAction : FSM_Action
{
    public override void Action(GameObject _destination)
    {
        // �����ߴٸ�
        if (_brain.SetDestination(_destination))
        {
            Debug.Log("���� �׼� ������");
            _brain._hand = _brain.interaction_Dic[deviceName].Interaction(_brain._hand);
            _brain.Reset();
        }
    }
}
