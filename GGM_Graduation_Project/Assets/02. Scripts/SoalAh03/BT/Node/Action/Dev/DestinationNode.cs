using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestinationNode : INode
{
    private AIManager brain;
    private AIStateType state;

    public DestinationNode(AIManager brain, AIStateType state)
    {
        this.brain = brain;
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

        return NodeState.Running;
    }

    public void OnEnd()
    {
    }
}
