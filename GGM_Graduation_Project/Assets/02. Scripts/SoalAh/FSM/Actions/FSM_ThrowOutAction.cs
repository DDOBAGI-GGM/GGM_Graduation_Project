using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_ThrowOutAction : FSM_Action
{
    public override void Action(GameObject _destination)
    {
        // �����ߴٸ�
        if (_brain.SetDestination(_destination))
        {
            Debug.Log("������ �׼� ������");
            _brain.interaction_Dic[deviceName].Interaction(_brain._hand);
            _brain.Reset();
        }
    }
}
