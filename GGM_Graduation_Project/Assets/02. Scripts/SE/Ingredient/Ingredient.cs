using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThreeIngredientType
{
    raw = 0,
    processing = 1,
    completion = 2,
    end = 3,
}

public enum TwoIngredientType
{
    raw = 0,
    completion = 1,
    end = 2,
}

public class Ingredient : MonoBehaviour
{
    /*
     ingredient : 특히 요리 등의) 재료, (…을 이루는 데 중요한) 구성 요소
     */

    [SerializeField] protected GameObject[] visual;           // 이러면 용량이 너무 많아 지지 않나?
    protected Vector3 createPos;

    // 변수 초기화해주고 오브젝트들 넣어주기
    public void Init<T>(T type)
    {
        createPos = transform.position;
        gameObject.name = gameObject.name.Substring(0, gameObject.name.IndexOf('('));
        visual = new GameObject[(int)type.GetType().GetField("end").GetValue(type)];
        for (int i = 0; i < visual.Length; i++)
        {
            visual[i] = gameObject.transform.GetChild(i).gameObject;
        }
    }

    public bool ChangeType<T>(T type)           // 재료 상태 변경해줌.
    {
        if (Convert.ToInt32(type) + 1 == (int)type.GetType().GetField("end").GetValue(type))
        // 타입 가져오고 그 타입에서 end 가 있는지 확인해서 그 필드(자료형?) 을 가져와서 필드 안 함수 GetValue 를 통해 값을 오브젝트로 받아와 인트로 언박싱 해준다.
        {
            Debug.Log(this.gameObject.name + " is completion.");       // 완성되어있다. 이미 끝 단계.
            return false;
        }
        //Debug.Log($"{Convert.ToInt32(before)} 는 현재, {Convert.ToInt32(after)} 는 바꿀것");
        visual[Convert.ToInt32(type)].SetActive(false);
        visual[Convert.ToInt32(type) + 1].SetActive(true);        // 오브젝트 끄고 켜주기

        Debug.Log("visual 외향 변경해주기");

        return true;
    }
}


// https://velog.io/@yongseok1000/%EC%9C%A0%EB%8B%88%ED%8B%B0-%EB%A6%AC%ED%94%8C%EB%A0%89%EC%85%98




// 다이나믹을 제네릭과 사용하기

/*class A<T>
{
    dynamic a = default(T); 
}*/