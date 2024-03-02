using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationNode : INode
{
    AI ai;

    public AnimationNode(AI ai)
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
        Debug.LogError("AnimationNode 진입");
        if (ai.agent.isStopped)
        {
            ai.animator.SetBool("Move", false);
            Debug.Log(ai.agent.isStopped + "  : 정지");
        }
        else
        {
            ai.animator.SetBool("Move", true);
            Debug.Log(ai.agent.isStopped + "  : 이동");
        }

        return NodeState.Success;

        // 현재 State에 따라 다른 애니메이션을 호출...

        switch (ai.stateType)
        {
            case AIStateType.Ingredient:
            {
                    return NodeState.Success;
            }
            case AIStateType.Processing:
                {

                    return NodeState.Success;
                }
            case AIStateType.Merge:
                {

                    return NodeState.Success;
                }
            case AIStateType.Attack:
                {

                    return NodeState.Success;
                }
            case AIStateType.Trash:
                {

                    return NodeState.Success;
                }
            default: return NodeState.Failure;
        }

        return NodeState.Success;
    }

    public void OnEnd()
    {
    }
}
