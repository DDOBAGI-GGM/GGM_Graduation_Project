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
        // recipe (so, int), �ڷ�� Ȯ���� ����...
        // recipe ����Ʈ ���빰�� ������ Ȯ���� �������� ���...
        // ������ 100�� �� ���͵� ����� ���ؼ� �ϴ� �ɷ�...
        // 20 70 50 30 �̶�� ������ 160,...

        int totalProbability = 0;
        foreach (RECIPE recipe in ai.manager.recipes)
            totalProbability += recipe.probability;

        int random = Random.Range(1, totalProbability);

        int gainProbability = 0;
        foreach (RECIPE recipe in ai.manager.recipes)
        {
            // ���� ��� Ȯ���� ���� �� ���� ũ�鼭 ������ Ȯ������ �۰ų� ���� ��
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
