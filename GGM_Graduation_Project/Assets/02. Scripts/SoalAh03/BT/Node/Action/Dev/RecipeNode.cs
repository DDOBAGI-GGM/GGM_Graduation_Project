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
        int random;

        if (ai.canFix)
        {
            random = Random.Range(1, 100);

            if (ai.manager.recovery.index >= random)
            {
                ai.recipe.recipe = ai.manager.recovery.recipe;
                ai.isRecovery = true;
                return NodeState.Success;
            }
        }

        int totalProbability = 0;
        foreach (RECIPE recipe in ai.manager.recipes)
        {
            totalProbability += recipe.index;
        }

        random = Random.Range(1, totalProbability);

        int gainProbability = 0;
        foreach (RECIPE recipe in ai.manager.recipes)
        {
            if (gainProbability < random && random <= gainProbability + recipe.index)
            {
                ai.recipe.recipe = recipe.recipe;
                return NodeState.Success;
            }

            gainProbability += recipe.index;
        }

        return NodeState.Failure;
    }

    public void OnEnd()
    {
    }
}
