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
        // ���� State�� ���� �ٸ� �ִϸ��̼��� ȣ��...

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
