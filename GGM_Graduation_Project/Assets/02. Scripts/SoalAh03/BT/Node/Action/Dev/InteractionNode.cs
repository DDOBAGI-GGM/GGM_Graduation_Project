using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;

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
        //GameObject item = ai.destination.GetComponent<IObject>().Interaction(ai.hand);

        //if (item != null)
        //{
        //    item.transform.position = ai.handPos.position;        // 오브젝트 손 위치로 이동
        //    item.transform.parent = ai.handPos;       // 손의 자식으로 설정
        //    ai.hand = item;
        //    return NodeState.Success;
        //}
        //return NodeState.Failure;

        ///////////////////// 재료, 가공, 병합의 과정을 나눈 버전...
        switch (((AIStateType)(ai.state + 1)).ToString())
        {
            case "Ingredient":
                {
                    GameObject item = ai.destination.GetComponent<IObject>().Interaction(ai.hand);
                    if (item != null)
                    {
                        item.transform.position = ai.handPos.position;        // 오브젝트 손 위치로 이동
                        item.transform.parent = ai.handPos;       // 손의 자식으로 설정
                        ai.hand = item;
                        return NodeState.Success;
                    }
                    return NodeState.Failure;
                }
            case "Processing":
                {
                    ai.destination.GetComponent<IObject>().Interaction(ai.hand);
                    if (ai.hand != null)
                    {
                        if (ai.hand.transform.GetChild(1).gameObject.activeSelf == true)
                            return NodeState.Success;
                        else
                            return NodeState.Running;
                    }
                    return NodeState.Failure;
                }
            case "Merge":
                {
                    Debug.Log(ai.hand);
                    GameObject item = ai.destination.GetComponent<IObject>().Interaction(ai.hand);
                    //MergeIngredient t = ai.destination.GetComponent<MergeIngredient>();
                    //if (t.one && t.two)
                    //{
                    //    ai.hand = null;
                    //    item = ai.destination.GetComponent<IObject>().Interaction(null);
                    //}
                    if (item != null)
                    {
                        item.transform.position = ai.handPos.position;        // 오브젝트 손 위치로 이동
                        item.transform.parent = ai.handPos;       // 손의 자식으로 설정
                        ai.hand = item;
                        Debug.Log(item.name);
                        return NodeState.Success;    
                    }
                    else
                        ai.hand = null;
                    return NodeState.Success;
                }
            case "Attack":
                Debug.Log("ttttttttttttttttttttttttttttt");
                ai.destination.GetComponent<IObject>().Interaction(ai.hand);
                return NodeState.Success;
            default: return NodeState.Failure;
        }

        return NodeState.Failure;
    }

    public void OnEnd()
    {
    }
}
