using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestinationNode : INode
{
    private AI ai;
    private AIStateType state;

    public DestinationNode(AI ai, AIStateType state)
    {
        this.ai = ai;
        this.state = state;
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
            if (obj.name == state.ToString())
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
