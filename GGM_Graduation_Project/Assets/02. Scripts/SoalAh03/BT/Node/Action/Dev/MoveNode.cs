using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode : INode
{
    private AI ai;
    private float speed;


    public MoveNode(AI ai, float speed = 5f)
    {
        this.ai = ai;
        this.speed = speed;
    }

    public void OnAwake()
    {
        ai.agent.speed = speed;
    }

    public void OnStart()
    {
        ai.agent.isStopped = false;
    }

    public NodeState Execute()
    {
        ai.agent.SetDestination(ai.destination.transform.position);

        if (!ai.agent.pathPending)
        {
            if (ai.agent.remainingDistance <= ai.agent.stoppingDistance)
            {
                if (!ai.agent.hasPath || ai.agent.velocity.sqrMagnitude == 3f)
                {
                    return NodeState.Success;
                }
            }
        }
        return NodeState.Running;
    }

    public void OnEnd()
    {
        ai.agent.isStopped = true;
    }
}
