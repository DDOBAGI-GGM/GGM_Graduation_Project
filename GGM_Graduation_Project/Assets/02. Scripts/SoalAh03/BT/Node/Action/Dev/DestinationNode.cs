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
        // state에 따른 목적지 설정 (state 이름과 obj 종류의 이름을 통일...) - 딕셔너리 쓸가...
        // 선택된 종류의 오브젝트 중 사용할 수 있는 것을 순회...
        // 고장이라면 사용하지 못 하고 사용할 수 있는게 2개 이상이라면 더 가까운 오브젝트 사용...

        return NodeState.Running;
    }

    public void OnEnd()
    {
    }
}
