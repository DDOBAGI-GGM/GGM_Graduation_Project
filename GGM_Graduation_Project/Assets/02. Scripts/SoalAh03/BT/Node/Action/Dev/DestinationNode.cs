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
                // 1. 고장 또는 사용중인지 / 2. 거리순
                target = ai.manager.objects[1].obj[0].item; // 임시
                break;
            }
            case AIStateType.Merge:
            {
                // 1. 고장 또는 사용중인지 / 2. 거리순
                target = ai.manager.objects[2].obj[0].item; // 임시
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
        }

        ai.destination = target;
        return NodeState.Success;
    }

    GameObject CheckRecipe()
    {
        string temp = ExtractName(ai.recipe.recipe[ai.recipeIdx]);
        string prefix = null;
        GameObject target = null;

        switch (temp)
        {
            case "completion":
            {
                prefix = ExtractPrefix(ai.recipe.recipe[ai.recipeIdx]);
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
            case "Enemy":
                Debug.LogError("미개발 ㅋ");
                break;
            default:
                Debug.LogError("이럴리가 없는데... ㄱㅗ$ㅈ3ㅑㅇ! ㅠㅡㅠ");
                break;
        }

        if (target == null)
            Debug.LogError("목적지를 설정할 수 없음");

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

    // 가장 가까운 오브젝트를 찾는...
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
            Debug.LogError("가장 가까운 목적지 설정 실패");
        return target;
    }

    public void OnEnd()
    {
    }
}
