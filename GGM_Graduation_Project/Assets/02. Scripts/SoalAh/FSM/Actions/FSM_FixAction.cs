using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_FixAction : FSM_Action
{
    public override void Action(GameObject _destination)
    {
        // �����ߴٸ�
        if (_brain.SetDestination(_destination))
        {
            Debug.Log("���� �׼� ������");
            _brain.Reset();
        }
    }
}
