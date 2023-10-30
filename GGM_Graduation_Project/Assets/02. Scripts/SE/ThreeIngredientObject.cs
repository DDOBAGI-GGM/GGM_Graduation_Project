using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeIngredientObject : Ingredient<ThreeIngredientType>
{
    [SerializeField] private GameObject ingredientPrefab;
    private Vector3 createPos;

    private void Awake()
    {
        createPos = transform.position;
        visual = new GameObject[(int)ThreeIngredientType.typeNumber];
        for (int i = 0; i < 3; i++)
        {
            visual[i] =  gameObject.transform.GetChild(i).gameObject;
        }
    }

    #region ��� ����
    public GameObject GetIngredient()
    {
        //Ǯ�� ����ϱ�
        GameObject ingredient = Instantiate(ingredientPrefab, createPos, Quaternion.identity);
        return ingredient;
    }
    #endregion

    #region ���ͷ���
    public void NextInteraction(float time)     // ���� �ܰ�� �̵��ϴ� ���ͷ���
    {
        StopCoroutine(InteractionRoutine(time));
        StartCoroutine(InteractionRoutine(time));
    }

    public IEnumerator InteractionRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        Type = Type + 1;
    }
    #endregion

    #region ��������
    public void GarbageCan(GameObject garbage)
    {
        //Ǯ�� ����ؼ� ���ֱ�
        Destroy(garbage);
    }
    #endregion

#if UNITY_EDITOR        // ����׿�
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            NextInteraction(2f);
        }
    }
#endif
}
