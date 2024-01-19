using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : INode
{
    private float waitTime;
    private bool randomWait;
    private float randomWaitMin;
    private float randomWaitMax;

    private float waitDuration;
    private float startTime;

    public WaitNode(float waitTime, bool randomWait = false, float randomWaitMin = 0, float randomWaitMax = 0)
    {
        this.waitTime = waitTime;
        this.randomWait = randomWait;
        this.randomWaitMin = randomWaitMin;
        this.randomWaitMax = randomWaitMax;
    }

    public void OnAwake()
    {
        waitDuration = 0f;
    }

    public void OnStart()
    {
        startTime = Time.time;
        if (randomWait)
            waitDuration = Random.Range(randomWaitMin, randomWaitMax);
        else
            waitDuration = waitTime;
    }

    public NodeState Execute()
    {
        if (startTime + waitDuration < Time.time)
        {
            return NodeState.Success;
        }
        return NodeState.Running;
    }

    public void OnEnd()
    {
        startTime = Time.time;
        waitDuration = 0f;
    }
}
