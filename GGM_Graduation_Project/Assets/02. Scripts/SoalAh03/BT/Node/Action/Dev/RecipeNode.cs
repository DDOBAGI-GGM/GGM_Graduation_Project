using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class RecipeNode : INode
{
    private AI ai;

    public RecipeNode(AI ai)
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
        if (ai.recipe != null)
            return NodeState.Failure;
        //else
        //{

        //} 
        return NodeState.Failure;
    }

    public void OnEnd()
    {
    }
}
