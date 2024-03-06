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
     ingredient : Ư�� �丮 ����) ���, (���� �̷�� �� �߿���) ���� ���
     */

    [SerializeField] protected GameObject[] visual;           // �̷��� �뷮�� �ʹ� ���� ���� �ʳ�?
    protected Vector3 createPos;

    // ���� �ʱ�ȭ���ְ� ������Ʈ�� �־��ֱ�
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

    public bool ChangeType<T>(T type)           // ��� ���� ��������.
    {
        if (Convert.ToInt32(type) + 1 == (int)type.GetType().GetField("end").GetValue(type))
        // Ÿ�� �������� �� Ÿ�Կ��� end �� �ִ��� Ȯ���ؼ� �� �ʵ�(�ڷ���?) �� �����ͼ� �ʵ� �� �Լ� GetValue �� ���� ���� ������Ʈ�� �޾ƿ� ��Ʈ�� ��ڽ� ���ش�.
        {
            Debug.Log(this.gameObject.name + " is completion.");       // �ϼ��Ǿ��ִ�. �̹� �� �ܰ�.
            return false;
        }
        //Debug.Log($"{Convert.ToInt32(before)} �� ����, {Convert.ToInt32(after)} �� �ٲܰ�");
        visual[Convert.ToInt32(type)].SetActive(false);
        visual[Convert.ToInt32(type) + 1].SetActive(true);        // ������Ʈ ���� ���ֱ�

        Debug.Log("visual ���� �������ֱ�");

        return true;
    }
}


// https://velog.io/@yongseok1000/%EC%9C%A0%EB%8B%88%ED%8B%B0-%EB%A6%AC%ED%94%8C%EB%A0%89%EC%85%98




// ���̳����� ���׸��� ����ϱ�

/*class A<T>
{
    dynamic a = default(T); 
}*/