using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
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
        int totalProbability = 0;
        foreach (RECIPE recipe in ai.manager.recipes)
            totalProbability += recipe.probability;

        int random = Random.Range(1, totalProbability);

        int gainProbability = 0;
        foreach (RECIPE recipe in ai.manager.recipes)
        {
            if (gainProbability < random && random <= gainProbability + recipe.probability)
            {
                ai.recipe = recipe.recipe;
                return NodeState.Success;
            }

            gainProbability += recipe.probability;
        }

        return NodeState.Failure;
    }

    public void OnEnd()
    {
    }
}
