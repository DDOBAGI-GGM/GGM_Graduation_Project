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
        string temp = ExtractName(ai.recipe.recipe[ai.recipeIdx]);
        GameObject target = null;
        switch (temp)
        {
            case "completion":
                {
                    temp = ExtractName(temp);
                    foreach (ITEM str in ai.manager.objects[0].obj)
                    {
                        if (str.name == temp)
                            target = str.item;
                    }
                    if (target == null)
                        Debug.LogError("�������� ������ �� ����");
                    return NodeState.Success;
                }
            case "Pot":
                {
                    foreach (ITEM str in ai.manager.objects[0].obj)
                    {
                        if (str.name == temp)
                            target = str.item;
                    }
                    if (target == null)
                        Debug.LogError("�������� ������ �� ����");
                    return NodeState.Success;
                }
            case "Floor":
            case "Object":
            case "Enemy":
                Debug.LogError("�̰��� ��");
                return NodeState.Failure;
            default:
                Debug.LogError("�̷����� ���µ�... ����$��3����! �ФѤ�");
                return NodeState.Failure;
        }
    }

    string ExtractName(string itemName)
    {
        string pattern = @"-_";
        Match match = Regex.Match(itemName, pattern);
        return match.Value;
    }

    public void OnEnd()
    {
    }
}
