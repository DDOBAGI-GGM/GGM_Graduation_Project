using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractionNode : INode
{
    private AI ai;
    private GameObject obj;

    public InteractionNode(AI ai, GameObject obj)
    {
        this.ai = ai;
        this.obj = obj;
    }

    public void OnAwake()
    {
    }

    public void OnStart()
    {
    }

    public NodeState Execute()
    {
        GameObject item = obj.GetComponent<IObject>().Interaction(ai.hand);
        if (item != null)
        {
            ai.hand = item;
            return NodeState.Success;
        }
        return NodeState.Failure;
    }

    public void OnEnd()
    {
    }
}
