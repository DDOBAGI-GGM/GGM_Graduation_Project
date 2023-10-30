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

    #region 재료 생성
    public GameObject GetIngredient()
    {
        //풀링 사용하기
        GameObject ingredient = Instantiate(ingredientPrefab, createPos, Quaternion.identity);
        return ingredient;
    }
    #endregion

    #region 인터랙션
    public void NextInteraction(float time)     // 다음 단계로 이동하는 인터랙션
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

    #region 쓰레기통
    public void GarbageCan(GameObject garbage)
    {
        //풀링 사용해서 해주기
        Destroy(garbage);
    }
    #endregion

#if UNITY_EDITOR        // 디버그용
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            NextInteraction(2f);
        }
    }
#endif
}
