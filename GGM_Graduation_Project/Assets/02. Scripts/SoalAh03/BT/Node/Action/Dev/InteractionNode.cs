using UnityEngine;

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
        switch (ai.stateType)
        {
            case AIStateType.Ingredient:
            {
                GameObject item = ai.destination.GetComponent<IObject>().Interaction(ai.hand);
                if (item != null)
                {
                    item.transform.position = ai.handPos.position;   
                    item.transform.parent = ai.handPos;    
                    ai.hand = item;
                    return NodeState.Success;
                }
                return NodeState.Failure;
            }
            case AIStateType.Processing:
            {
                ai.destination.GetComponent<IObject>().Interaction(ai.hand);
                if (ai.hand != null)
                {
                    if (ai.hand.transform.GetChild(1).gameObject.activeSelf == true)
                    {
                        return NodeState.Success;
                    }
                    else
                        return NodeState.Running;
                }
                return NodeState.Failure;
            }
            case AIStateType.Merge:
            {
                GameObject item = ai.destination.GetComponent<IObject>().Interaction(ai.hand);
                if (item != null)
                {
                    item.transform.position = ai.handPos.position;     
                    item.transform.parent = ai.handPos;  
                    ai.hand = item;
                    return NodeState.Success;    
                }
                else
                    ai.hand = null;
                return NodeState.Success;
            }
            case AIStateType.Attack:
            {
                ai.destination.GetComponent<IObject>().Interaction(ai.hand);
                ai.hand = null;
                return NodeState.Success;
            }
            case AIStateType.Trash:
            {
                ai.destination.GetComponent<IObject>().Interaction(ai.hand);
                ai.hand = null;
                return NodeState.Success;
            }
            default: return NodeState.Failure;
        }
    }

    public void OnEnd()
    {
    }
}
