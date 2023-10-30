using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThreeIngredientType
{
    raw = 0,
    processing = 1,
    completion = 2,
    typeNumber = 3,
}

public enum TwoIngredientType
{
    raw = 0,
    completion = 1,
    typeNumber = 2,
}

public class Ingredient<T> : MonoBehaviour where T : struct      // 이넘값만 받아오깅
{
    /*
     ingredient : 특히 요리 등의) 재료, (…을 이루는 데 중요한) 구성 요소
     */

    [SerializeField] T type;     // 재료의 상태를 정할 때
    [SerializeField] protected GameObject[] visual;           // 이러면 용량이 너무 많아 지지 않나?

   public T Type
    {
        get { return type; }
        set 
        { 
            type = ChangeType(type, value);
        }
    }

    public T ChangeType<T>(T before, T after)           // 재료 상태 변경해줌.
    {
        if (Convert.ToInt32(type) + 1 == (int)type.GetType().GetField("typeNumber").GetValue(type))
        {
            Debug.LogError(this.gameObject.name + " is completion.");
            return before;
        }

        Debug.Log($"{Convert.ToInt32(before)} 는 현재, {Convert.ToInt32(after)} 는 바꿀것");
        visual[Convert.ToInt32(before)].SetActive(false);
        visual[Convert.ToInt32(after)].SetActive(true);        // 일반화 하기

        Debug.Log("visual 외향 변경해주기");
        return after;
    }
}
