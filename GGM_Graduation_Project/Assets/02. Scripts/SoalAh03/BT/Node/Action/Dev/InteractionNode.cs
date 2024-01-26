using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class InteractionNode : INode
{
    private AI ai;

    INode wait = new WaitNode(3f);

    public InteractionNode(AI ai)
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
        GameObject item = ai.destination.GetComponent<IObject>().Interaction(ai.hand);

        if (item != null)
            return NodeState.Success;
        return NodeState.Failure;

        /////////////////////// 재료, 가공, 병합의 과정을 나눈 버전...
        //Debug.Log(ai.test);
        //switch (((AIStateType)(ai.state + 1)).ToString())
        //{
        //    case "Ingredient":
        //{
        //        GameObject item = ai.destination.GetComponent<IObject>().Interaction(ai.hand);
        //        if (item != null)
        //        {
        //            item.transform.position = ai.handPos.position;        // 오브젝트 손 위치로 이동
        //            item.transform.parent = ai.handPos;       // 손의 자식으로 설정
        //            ai.hand = item;
        //            return NodeState.Success;
        //        }
        //        return NodeState.Failure;
        //    }
        //    case "Processing":
        //    {
        //        GameObject item = ai.destination.GetComponent<IObject>().Interaction(ai.hand);
        //        //if (item != null)
        //        //    return NodeState.Success;
        //        return NodeState.Success;
        //        //return wait.Execute() == NodeState.Success ? NodeState.Success : NodeState.Failure;
        //    }
        //    case "Merge":
        //    {
        //        GameObject item = ai.destination.GetComponent<IObject>().Interaction(ai.hand);
        //        if (item != null)
        //        {
        //            item.transform.position = ai.handPos.position;        // 오브젝트 손 위치로 이동
        //            item.transform.parent = ai.handPos;       // 손의 자식으로 설정
        //            ai.hand = item;
        //            return NodeState.Success;
        //        }
        //        return NodeState.Failure;
        //    }
        //    default: return NodeState.Failure;
        //}

        //return NodeState.Failure;
    }

    public void OnEnd()
    {
    }
}
