using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private IngredientType inputType = IngredientType.raw;        // 들어와야 하는 재료의 단계

    private Vector3 createPos;

    private void Awake()
    {
        createPos = transform.position;
    }
    #region 재료 생성
    public GameObject GetIngredient()
    {
        // 풀링 사용하기
        GameObject ingredient = Instantiate(ingredientPrefab, createPos, Quaternion.identity);
        return ingredient;
    }
    #endregion

    #region 인터랙션
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
        if (type.Type != inputType)        // 내가 정해준 레벨이 아니면
        {
            Debug.LogError(this.gameObject.name + "is unsuitable ingredient");
            yield break;
        }

        yield return new WaitForSeconds(time);
       type.Type = type.Type + 1;
    }
    #endregion

    #region 쓰레기통
    public void GarbageCan(GameObject garbage)
    {
        // 풀링 사용해서 해주기
        Destroy(garbage);
    }
    #endregion
}