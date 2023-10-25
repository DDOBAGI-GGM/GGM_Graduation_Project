using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private IngredientType inputType = IngredientType.raw;        // ���;� �ϴ� ����� �ܰ�

    private Vector3 createPos;

    private void Awake()
    {
        createPos = transform.position;
    }
    #region ��� ����
    public GameObject GetIngredient()
    {
        // Ǯ�� ����ϱ�
        GameObject ingredient = Instantiate(ingredientPrefab, createPos, Quaternion.identity);
        return ingredient;
    }
    #endregion

    #region ���ͷ���
    public void Interaction(Ingredient type, float time)
    {
        StopCoroutine(InteractionRoutine(type, time));
        StartCoroutine(InteractionRoutine(type, time));
    }

    public IEnumerator InteractionRoutine(Ingredient type, float time)
    {
        if ((int)type.Type == 2)
        {
            Debug.LogError(type.gameObject.name + "is completion.");
            yield break;
        }
        if (type.Type != inputType)        // ���� ������ ������ �ƴϸ�
        {
            Debug.LogError(this.gameObject.name + "is unsuitable ingredient");
            yield break;
        }

        yield return new WaitForSeconds(time);
       type.Type = type.Type + 1;
    }
    #endregion

    #region ��������
    public void GarbageCan(GameObject garbage)
    {
        // Ǯ�� ����ؼ� ���ֱ�
        Destroy(garbage);
    }
    #endregion
}