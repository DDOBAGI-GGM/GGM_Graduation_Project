using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopState : State<AIController>
{
    public override void OnAwake()
    {
        stateMachineClass.navAgent.isStopped = true;
        stateMachineClass.navAgent.speed = 0f;
    }

    public override void OnStart()
    {
        stateMachineClass.navAgent.isStopped = true;
        stateMachineClass.navAgent.speed = 0f;
    }

    public override void OnUpdate(float deltaTime)
    {
        
    }

    public override void OnEnd()
    {
        stateMachineClass.navAgent.isStopped = true;
    }
}
