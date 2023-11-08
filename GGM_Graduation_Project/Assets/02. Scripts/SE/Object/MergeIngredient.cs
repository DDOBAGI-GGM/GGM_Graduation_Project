using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MergeIngredient : MonoBehaviour, IObject
{
    private bool interactive = false;

    private SortedSet<string> inputs = new SortedSet<string>();            // 내가 넣은 재료 정렬
    private SortedSet<string>[] recipeSet;      // 재료 정렬
    private bool one, two;      // 이렇게 해서 만들어주기?

    private PlayerFOV playerFOV;
    private PlayerInteraction playerInteraction;

    [SerializeField] private Transform[] pen = new Transform[2];
    [SerializeField] private RecipeListSO[] recipes;

    private void Awake()
    {
        recipeSet = new SortedSet<string>[recipes.Length];       // 레시피SO 개수랑 같음.
        for (int i = 0; i < recipeSet.Length; i++)      // 레시피Set 개수만큼
        {
            for (int j = 0; j < recipes[i].recipe.Count; j++)       // 레시피SO 속에 레시피 리스트의 카운트 만큼.
            {
                recipeSet[i].Add(recipes[i].recipe[j]);             // 지금 레시피 set에서 레시피SO 속에 레시피 j를 넣어줌.
            }
        }
    }

    public GameObject Interaction(GameObject ingredient)
    {
        if (interactive && (!one || !two))
        {
            Debug.Log("재료합치기");
            playerInteraction.CurrentObjectInHand = null;       // 손에서 내려두고
            ingredient.transform.parent = this.transform;       // 부모 설정해주고
            if (!one)       // 첫번째에 재료가 없으면
            {
                one = true;
                ingredient.transform.position = pen[0].transform.position;
            }
            else if (one && !two)       // 두번째에 재료가 없으면
            {
                two = true;
                ingredient.transform.position = pen[1].transform.position;
            }
            inputs.Add(ingredient.name);            // 이렇게 받아와주는데 후 수정가능하게? 이름 수정하기잉 
        }
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("F키로 상호작용이 가능해요!");
            // 상호작용 가능 표시해주기
            if (playerFOV == null)
            {
                playerFOV = other.gameObject.GetComponent<PlayerFOV>();
                playerInteraction = other.gameObject.GetComponent<PlayerInteraction>();
            }
            if (playerFOV.CheckForObjectsInView().name == gameObject.transform.name)
            {
                interactive = true;
                // Debug.Log("플레이어 인터랙션 시 재료를 버려요.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactive = false;
        }
    }

    private void LateUpdate()
    {
        if (one && two)
        {
            // 레시피 확인해주기
            foreach (SortedSet<string> recipe in recipeSet)
            {

            }


            one = two = false;      // 둘다 펄스
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //if (UnityEditor.Selection.activeObject == gameObject)
        //{
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, 1);
        Gizmos.color = Color.green;
        //}
    }
#endif
}
