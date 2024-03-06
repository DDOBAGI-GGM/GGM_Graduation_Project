using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class DestinationNode : INode
{
    private AI ai;
    private GameObject target = null;
    private string baseIngredient, recovery1, recovery2;

    public DestinationNode(AI ai)
    {
        this.ai = ai;
    }

    public void OnAwake()
    {
        baseIngredient = ExtractName(ai.manager.recovery.recipe.recipe[0]);
        recovery1 = ExtractName(ai.manager.recovery.recipe.recipe[0]);
        recovery2 = ExtractName(ai.manager.recovery.recipe.recipe[1]);
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
                target = CheckRecipe(ai.recipe.recipe.recipe[ai.recipe.index]);
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

    GameObject CheckRecipe(string tttt)
    {
        string temp = ExtractName(tttt);
        string prefix = null;

        if (temp == "completion")
        {
            prefix = ExtractPrefix(tttt);
            foreach (ITEM str in ai.manager.objects[0].obj)
            {
                if (str.name == prefix)
                    target = str.item;
            }
        }
        else if (temp == "Base")
        {
            foreach (ITEM str in ai.manager.objects[0].obj)
            {
                if (str.name == temp)
                    target = str.item;
            }
        }
        else if (temp == recovery1 || temp == recovery2)
        {
            foreach (RECIPE a in ai.manager.recipes)
            {
                if (a.recipe.name == temp)
                {
                    ai.oldRecipe.recipe = a.recipe;
                    CheckRecipe(ai.oldRecipe.recipe.recipe[ai.oldRecipe.index]);
                }
            }
        }
        else
            Debug.LogError("올바르지 않은 레시피 값이 읽혔다 : " + temp);

        if (target == null)
            Debug.LogError("목적지를 설정할 수 없음");

        return target;
    }

    string ExtractName(string itemName)
    {
        return itemName.Split('_')[1];
    }

    string ExtractPrefix(string itemName)
    {
        return itemName.Split('_')[0];
    }

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
