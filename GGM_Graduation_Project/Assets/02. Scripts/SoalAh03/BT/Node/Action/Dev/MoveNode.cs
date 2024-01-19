using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MoveNode : MonoBehaviour
{
    NavMeshAgent agent;

    private Transform destination;
    private float speed;


    public MoveNode(Transform destination, float speed = 5f)
    {
        this.destination = destination;
        this.speed = speed;
    }

    public void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void OnStart()
    {
        agent.speed = speed;
        agent.isStopped = false;
        agent.SetDestination(agent.destination);
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

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return NodeState.Success;
                }
            }
        }
        return NodeState.Running;
    }

    public void OnEnd()
    {
        agent.isStopped = true;
    }
}
