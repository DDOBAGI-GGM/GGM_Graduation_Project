using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_AttackAction : FSM_Action
{
    public override void Action()
    {
        // �� �� �׼��� ���� �����ϸ� state none���� �ٲٱ�
        _brain.ChangeState(null);
    }
}
