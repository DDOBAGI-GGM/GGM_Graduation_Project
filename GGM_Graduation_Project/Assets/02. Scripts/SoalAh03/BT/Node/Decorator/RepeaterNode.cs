using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class RepeaterNode : INode
{
    private INode node;
    private bool forever;
    private bool time;
    private int count;

    private int currentCount;
    private float currentTime;

    public RepeaterNode(INode node, bool forever, bool time = false, int count = 1)
    {
        this.node = node;
        this.forever = forever;
        this.time = time;
        this.count = count;
    }

    public void OnAwake()
    {
    }

    public void OnStart()
    {
        currentTime = Time.time;
        currentCount = count;
    }

    public NodeState Execute()
    {
        // ���� �ʿ���, ������� �� ��
        if (forever)
        {
            //while (forever)
            //{
            //    NodeState result = node.Execute();
            //    if (result == NodeState.Failure)
            //        return NodeState.Failure;
            //}

            NodeState result = node.Execute();
            if (result == NodeState.Failure)
                return NodeState.Failure;
            return NodeState.Running;
        }
        else
        {
            if (time)
            {
                if (currentTime + count < Time.time)
                {
                    return NodeState.Success;
                }
                return NodeState.Running;
            }
            else
            {
                while (currentCount <= count)
                {
                    NodeState result = node.Execute();
                    if (result == NodeState.Failure)
                        return NodeState.Failure;
                    currentCount++;
                }
                return NodeState.Success;
            }
        }
    }

    public void OnEnd()
    {
        currentTime = 0f;
    }
}
