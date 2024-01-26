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
    }

    public void OnStart()
    {
        ai.agent.speed = speed;
    }

    public NodeState Execute()
    {
        ai.agent.SetDestination(ai.destination.transform.position);
        //float distance = Vector3.Distance(ai.transform.position, ai.destination.transform.position);
        //Vector3 dir = ai.destination.transform.position - ai.transform.position;
        //RaycastHit hit;
        //if (Physics.Raycast(ai.transform.position, dir, out hit)
        //    && distance <= 5f && hit.transform.CompareTag("Object"))
        //{
        //    if (hit.collider.gameObject.name == ai.destination.gameObject.name)
        //        return NodeState.Success;
        //}
        //else
        //    return NodeState.Running;
        //ai.agent.SetDestination(ai.agent.destination);

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
        //ai.agent.isStopped = true;
    }
}
