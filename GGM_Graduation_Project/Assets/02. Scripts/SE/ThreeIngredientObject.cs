using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeIngredientObject : Ingredient<ThreeIngredientType>
{
    [SerializeField] private GameObject ingredientPrefab;

    private void Awake()
    {
        base.Awake();
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
