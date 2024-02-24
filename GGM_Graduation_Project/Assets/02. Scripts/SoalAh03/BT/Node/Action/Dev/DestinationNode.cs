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
                target = CheckRecipe(ai.recipe.recipe.recipe[ai.recipe.index]);
                    //Debug.Log(target.ToString());
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
        //string test = "";

        //if (ai.twoRecipe)
        //    test = ai.oldRecipe.recipe.recipe[ai.oldRecipe.index];
        //else
        //    test = ai.recipe.recipe.recipe[ai.recipe.index];

        //if (ai.recipe != null)
        //    test = ai.recipe.recipe.recipe[ai.recipe.index];
        //else
        //    test = ai.oldRecipe.recipe.recipe[ai.oldRecipe.index];

        string temp = ExtractName(tttt);
        //string temp = ExtractName(ai.recipe.recipe.recipe[ai.recipe.index]);
        //string temp = ExtractName(test);
        string prefix = null;
        

        switch (temp)
        {
            case "completion":
                {
                    //prefix = ExtractPrefix(ai.recipe.recipe.recipe.recipe[ai.recipe.recipe.index]);
                    //foreach (ITEM str in ai.manager.objects[0].obj)
                    //{
                    //    if (str.name == prefix)
                    //        target = str.item;
                    //}

                    prefix = ExtractPrefix(tttt);
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
                    // recovery라면 현재 레시피(회복)는 oldrecipe에 옮겨 저장
                    // 현재 recipe는 recovery의 
                    //ai.oldRecipe = ai.recipe;
                    foreach (RECIPE a in ai.manager.recipes)
                    {
                        //string ss = ExtractName(ai.recipe.recipe.recipe[ai.recipe.index]);
                        if (a.recipe.name == temp/*ss*/)
                        {
                            ai.oldRecipe.recipe = a.recipe;
                            CheckRecipe(ai.oldRecipe.recipe.recipe[ai.oldRecipe.index]);
                        }
                    }
                    //ai.recipe.recipe = ai.oldRecipe.recipe[ai.oldRecipe.index];
                    // ai recipe 자체에 알맞는 so를 찾아서 넣어줘야함...
                }
                //case "Enemy":
                break;
            default:
                Debug.LogError("이럴리가 없는데... ㄱㅗ$ㅈ3ㅑㅇ! ㅠㅡㅠ");
                break;
        }

        //if (target == null)
        //    Debug.LogError("목적지를 설정할 수 없음");

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

    // 가장 가까운 오브젝트를 찾는... (테스트 성공) + 고장 또는 사용중인지 선별하기...
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
