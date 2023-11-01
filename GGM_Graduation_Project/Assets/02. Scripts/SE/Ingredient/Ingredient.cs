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

    protected Vector3 createPos;

    public T Type
    {
        get { return type; }
        set 
        { 
            type = ChangeType(type, value);
        }
    }

    // 변수 초기화해주고 오브젝트들 넣어주기
    public void Awake()
    {
        createPos = transform.position;
        visual = new GameObject[(int)type.GetType().GetField("typeNumber").GetValue(type)];
        for (int i = 0; i < visual.Length; i++)
        {
            visual[i] = gameObject.transform.GetChild(i).gameObject;
        }
    }

    public T ChangeType<T>(T before, T after)           // 재료 상태 변경해줌.
    {
        if (Convert.ToInt32(type) + 1 == (int)type.GetType().GetField("typeNumber").GetValue(type))
        // 타입 가져오고 그 타입에서 typeNumber 가 있는지 확인해서 그 필드(자료형?) 을 가져와서 필드 안 함수 GetValue 를 통해 값을 오브젝트로 받아와 인트로 언박싱 해준다.
        {
            Debug.LogError(this.gameObject.name + " is completion.");       // 완성되어있다. 이미 끝 단계.
            return before;
        }

        //Debug.Log($"{Convert.ToInt32(before)} 는 현재, {Convert.ToInt32(after)} 는 바꿀것");
        visual[Convert.ToInt32(before)].SetActive(false);
        visual[Convert.ToInt32(after)].SetActive(true);        // 오브젝트 꺼주기

        //Debug.Log("visual 외향 변경해주기");
        return after;
    }
}


// https://velog.io/@yongseok1000/%EC%9C%A0%EB%8B%88%ED%8B%B0-%EB%A6%AC%ED%94%8C%EB%A0%89%EC%85%98