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

   public T Type
    {
        get { return type; }
        set 
        { 
            type = ChangeType(type, value);
        }
    }

    public T ChangeType<T>(T before, T after)           // ��� ���� ��������.
    {
        if (Convert.ToInt32(type) + 1 == (int)type.GetType().GetField("typeNumber").GetValue(type))
        {
            Debug.LogError(this.gameObject.name + " is completion.");
            return before;
        }

        Debug.Log($"{Convert.ToInt32(before)} �� ����, {Convert.ToInt32(after)} �� �ٲܰ�");
        visual[Convert.ToInt32(before)].SetActive(false);
        visual[Convert.ToInt32(after)].SetActive(true);        // �Ϲ�ȭ �ϱ�

        Debug.Log("visual ���� �������ֱ�");
        return after;
    }
}
