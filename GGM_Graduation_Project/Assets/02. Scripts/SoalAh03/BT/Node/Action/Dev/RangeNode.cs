using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : INode
{
    AI ai;
    public RangeNode(AI ai)
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
        // 목적지랑 거리가 5 이하라면
        if (ai.destination == null)
            return NodeState.Failure;

        float distance = Vector3.Distance(ai.transform.position , ai.destination.transform.position);
        if (distance < 2f)
            return NodeState.Success;
        return NodeState.Failure;

        //Vector3 dir = ai.destination.transform.position - ai.transform.position;
        //if (Physics.Raycast(current.position, dir, out hit)
        //    && distance <= 5f && hit.transform.CompareTag("Object"))
        //{
        //    Debug.Log("여기까지 온다");
        //    return NodeState.SUCCESS;
        //}
        //else
        //    return NodeState.FAILURE;
    }

    public void OnEnd()
    {
    }
}
