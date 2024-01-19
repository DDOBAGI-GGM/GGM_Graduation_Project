using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode : INode
{
    private AI ai;
    private GameObject destination;
    //private Transform destination;
    private float speed;


    public MoveNode(AI ai, GameObject destination/*Transform destination*/, float speed = 5f)
    {
        this.ai = ai;
        this.destination = destination;
        this.speed = speed;
    }

    public void OnAwake()
    {
    }

    public void OnStart()
    {
        ai.agent.speed = speed;
        ai.agent.isStopped = false;
        ai.agent.SetDestination(ai.agent.destination);
    }

    public NodeState Execute()
    {
        {
        //float distance = Vector3.Distance(transform.position, destination.position);
        //Vector3 dir = destination.position - transform.position;
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, dir, out hit)
        //    && distance <= 5f && hit.transform.CompareTag("Object"))
        //{
        //    return NodeState.Success;
        //}
        //else
        //    return NodeState.Running;
        }

        if (!ai.agent.pathPending)
        {
            if (ai.agent.remainingDistance <= ai.agent.stoppingDistance)
            {
                if (!ai.agent.hasPath || ai.agent.velocity.sqrMagnitude == 0f)
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
