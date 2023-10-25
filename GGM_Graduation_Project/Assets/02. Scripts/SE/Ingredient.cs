using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    raw = 0,
    processing = 1,
    completion = 2
}

public class Ingredient : MonoBehaviour
{
    /*
     ingredient : Ư�� �丮 ����) ���, (���� �̷�� �� �߿���) ���� ���
     */
 
    [SerializeField] private IngredientType type;
    public IngredientType Type
    {
        get { return type; }
        set 
        { 
            ChangeType(type, value);
            type = value;
        }
    }
    [SerializeField] private GameObject[] visual = new GameObject[3];           // �̷��� �뷮�� �ʹ� ���� ���� �ʳ�?

    public void ChangeType(IngredientType before, IngredientType after)
    {
        visual[(int)before].SetActive(false);
        visual[(int)after].SetActive(true);
        Debug.Log("visual ���� �������ֱ�");
    }

#if UNITY_EDITOR        // ����׿�
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("raw");
            Type = IngredientType.raw;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("processing");
            Type = IngredientType.processing;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("completion");
            Type = IngredientType.completion;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Interaction");
            FindObjectOfType<IngredientObject>().Interaction(this, 1f);
        }
    }
#endif
}
