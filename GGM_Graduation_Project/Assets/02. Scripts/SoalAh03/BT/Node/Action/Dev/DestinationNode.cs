using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class DestinationNode : INode
{
    private AI ai;

    public DestinationNode(AI ai)
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
        GameObject target = null;

        switch (ai.stateType)
        {
            case AIStateType.Ingredient:
            {
                target = CheckRecipe();
                break;
            }
            case AIStateType.Processing:
            {
                target = Closest(ai.manager.objects[1].obj);
                break;
            }
            case AIStateType.Merge:
            {
                target = Closest(ai.manager.objects[2].obj);
                break;
            }
            case AIStateType.Attack:
            {
                target = ai.manager.objects[3].obj[0].item;
                break;
            }
            case AIStateType.Trash:
            {
                target = ai.manager.objects[4].obj[0].item;
                break;
            }
            case AIStateType.Shelf:
            {
                target = ai.manager.objects[5].obj[0].item;
                break;
            }
        }

        ai.destination = target;
        return NodeState.Success;
    }

    GameObject CheckRecipe()
    {
        string temp = ExtractName(ai.recipe.recipe.recipe.recipe[ai.recipe.recipe.index]);
        string prefix = null;
        GameObject target = null;

        switch (temp)
        {
            case "completion":
                {
                    prefix = ExtractPrefix(ai.recipe.recipe.recipe.recipe[ai.recipe.recipe.index]);
                    foreach (ITEM str in ai.manager.objects[0].obj)
                    {
                        if (str.name == prefix)
                            target = str.item;
                    }
                    break;
                }
            case "Pot":
                {
                    foreach (ITEM str in ai.manager.objects[0].obj)
                    {
                        if (str.name == temp)
                            target = str.item;
                    }
                    break;
                }
            case "Floor":
            case "Object":
                {
                    // recovery��� ���� ������(ȸ��)�� oldrecipe�� �Ű� ����
                    // ���� recipe�� recovery�� 
                    //ai.oldRecipe = ai.recipe;
                    //foreach (RECIPE test in ai.manager.recipes)
                    //{
                    //    string ss = ExtractName(ai.oldRecipe.recipe[ai.oldRecipeIdx]);
                    //    if (test.recipe.name == ss)
                    //    {
                    //        ai.recipe = test.recipe;
                    //    }
                    //}
                    //ai.isRecovery = true;

                    ai.recipe.oldRecipe = ai.recipe.recipe;
                    ai.recipe.recipe = null; // ��?

                }
                //case "Enemy":
                break;
            default:
                Debug.LogError("�̷����� ���µ�... ����$��3����! �ФѤ�");
                break;
        }

        //if (target == null)
        //    Debug.LogError("�������� ������ �� ����");

        return target;
    }

    string ExtractName(string itemName)
    {
        string pattern = @"-";
        Match match = Regex.Match(itemName, pattern);
        return itemName.Split('-')[1];
    }

    string ExtractPrefix(string itemName)
    {
        return itemName.Split('-')[0];
    }

    // ���� ����� ������Ʈ�� ã��... (�׽�Ʈ ����) + ���� �Ǵ� ��������� �����ϱ�...
    GameObject Closest(List<ITEM> objs)
    {
        GameObject target = null;
        float minDistance = 0;

        foreach (ITEM obj in objs)
        {
            float distance = Vector3.Distance(ai.transform.position, obj.item.transform.position);
            if (minDistance > distance || minDistance == 0)
            {
                minDistance = distance;
                target = obj.item;
            }
        }

        if (target == null)
            Debug.LogError("���� ����� ������ ���� ����");
        return target;
    }

    public void OnEnd()
    {
    }
}
