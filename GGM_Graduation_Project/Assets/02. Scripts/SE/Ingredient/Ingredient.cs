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

public class Ingredient<T> : MonoBehaviour where T : struct      // �̳Ѱ��� �޾ƿ���
{
    /*
     ingredient : Ư�� �丮 ����) ���, (���� �̷�� �� �߿���) ���� ���
     */

    [SerializeField] T type;     // ����� ���¸� ���� ��
    [SerializeField] protected GameObject[] visual;           // �̷��� �뷮�� �ʹ� ���� ���� �ʳ�?

    protected Vector3 createPos;

    public T Type
    {
        get { return type; }
        set 
        { 
            type = ChangeType(type, value);
        }
    }

    // ���� �ʱ�ȭ���ְ� ������Ʈ�� �־��ֱ�
    public void Awake()
    {
        createPos = transform.position;
        visual = new GameObject[(int)type.GetType().GetField("typeNumber").GetValue(type)];
        for (int i = 0; i < visual.Length; i++)
        {
            visual[i] = gameObject.transform.GetChild(i).gameObject;
        }
    }

    public T ChangeType<T>(T before, T after)           // ��� ���� ��������.
    {
        if (Convert.ToInt32(type) + 1 == (int)type.GetType().GetField("typeNumber").GetValue(type))
        // Ÿ�� �������� �� Ÿ�Կ��� typeNumber �� �ִ��� Ȯ���ؼ� �� �ʵ�(�ڷ���?) �� �����ͼ� �ʵ� �� �Լ� GetValue �� ���� ���� ������Ʈ�� �޾ƿ� ��Ʈ�� ��ڽ� ���ش�.
        {
            Debug.LogError(this.gameObject.name + " is completion.");       // �ϼ��Ǿ��ִ�. �̹� �� �ܰ�.
            return before;
        }

        //Debug.Log($"{Convert.ToInt32(before)} �� ����, {Convert.ToInt32(after)} �� �ٲܰ�");
        visual[Convert.ToInt32(before)].SetActive(false);
        visual[Convert.ToInt32(after)].SetActive(true);        // ������Ʈ ���ֱ�

        //Debug.Log("visual ���� �������ֱ�");
        return after;
    }
}


// https://velog.io/@yongseok1000/%EC%9C%A0%EB%8B%88%ED%8B%B0-%EB%A6%AC%ED%94%8C%EB%A0%89%EC%85%98