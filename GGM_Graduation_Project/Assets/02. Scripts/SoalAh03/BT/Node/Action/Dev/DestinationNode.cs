using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestinationNode : INode
{
    private AI ai;

    public DestinationNode(AI ai)
    {
        this.ai = ai;
    }

    public void OnAwake()
    {
    }

    public void OnStart()
    {
    }

    public NodeState Execute()
    {
        // state�� ���� ������ ���� (state �̸��� obj ������ �̸��� ����...) - ��ųʸ� ����...
        // ���õ� ������ ������Ʈ �� ����� �� �ִ� ���� ��ȸ...
        // �����̶�� ������� �� �ϰ� ����� �� �ִ°� 2�� �̻��̶�� �� ����� ������Ʈ ���...


        foreach (OBJ obj in ai.manager.objects)
        {
            if (obj.name == ((AIStateType)(ai.state + 1)).ToString())
            {
                foreach (GameObject item in obj.obj)
                {
                    if (item.activeSelf == true)
                    {
                        ai.destination = item;
                    }
                }
            }
        }

        return NodeState.Success;
    }

    public void OnEnd()
    {
    }
}
