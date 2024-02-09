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
        // recipe (so, int), 자료와 확률로 구성...
        // recipe 리스트 내용물의 개수와 확률의 총합으로 계산...
        // 총합이 100이 안 나와도 백분율 구해서 하는 걸로...
        // 20 70 50 30 이라면 총합은 160,...

        int totalProbability = 0;
        foreach (RECIPE recipe in ai.manager.recipes)
            totalProbability += recipe.probability;

        int random = Random.Range(1, totalProbability);

        int gainProbability = 0;
        foreach (RECIPE recipe in ai.manager.recipes)
        {
            // 이전 모든 확률을 더한 것 보다 크면서 현재의 확률보다 작거나 같을 때
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
