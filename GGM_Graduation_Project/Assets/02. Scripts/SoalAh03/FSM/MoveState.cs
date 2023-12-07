using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : State<AIController>
{
    public override void OnAwake()
    {
        stateMachineClass.navAgent.isStopped = true;
        stateMachineClass.navAgent.speed = 0f;
    }

    public override void OnStart()
    {
        stateMachineClass.navAgent.isStopped = false;
        stateMachineClass.navAgent.speed = stateMachineClass.brain.speed;
    }

    public override void OnUpdate(float deltaTime)
    {
        stateMachineClass.navAgent.SetDestination(stateMachineClass.target.position);

        if (!stateMachineClass.navAgent.pathPending)
        {
            if (stateMachineClass.navAgent.remainingDistance <= stateMachineClass.navAgent.stoppingDistance)
            {
                if (!stateMachineClass.navAgent.hasPath || stateMachineClass.navAgent.velocity.sqrMagnitude == 0f)
                {
                    stateMachineClass.tArride = true; // µµÂøÀÌ¿ë
                }
            }
        }
    }

    public override void OnEnd()
    {
        stateMachineClass.navAgent.isStopped = true;
    }
}
