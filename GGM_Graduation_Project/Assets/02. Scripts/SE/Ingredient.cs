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
     ingredient : 특히 요리 등의) 재료, (…을 이루는 데 중요한) 구성 요소
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
    [SerializeField] private GameObject[] visual = new GameObject[3];           // 이러면 용량이 너무 많아 지지 않나?

    public void ChangeType(IngredientType before, IngredientType after)
    {
        visual[(int)before].SetActive(false);
        visual[(int)after].SetActive(true);
        Debug.Log("visual 외향 변경해주기");
    }

#if UNITY_EDITOR        // 디버그용
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
