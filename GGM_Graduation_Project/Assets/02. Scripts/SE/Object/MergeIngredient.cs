using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(SphereCollider))]
public class MergeIngredient : MonoBehaviour, IObject
{
    private bool interactive = false;
    public bool Interactive { get { return interactive; } private set { } }

    private List<string> inputList = new List<string> ();           // 내가 넣은 재료들
    private List<string>[] recipeList;          // 레시피들 3가지
    private bool one, two, result = false;      // 이렇게 해서 만들어주기?
    public bool Result { get { return result; } private set { } }

    private PlayerFOV playerFOV;
    private PlayerInteraction playerInteraction;

    [SerializeField] private Transform[] pen = new Transform[2];
    [SerializeField] private RecipeListSO[] recipes;
    [SerializeField] private GameObject garbagePrefab;
    private GameObject resultObject = null;        // 완성품이 있는지, 있으면 플레이어가 상호작용 시 완성품을 주어야함

    private void Awake()
    {
        recipeList = new List<string>[recipes.Length];       // 레시피SO 개수랑 같음.
        //Debug.Log(recipeList.Length);

        for (int i = 0; i < recipeList.Length; i++)
        {
            recipeList[i] = new List<string>(); // 각 요소에 List<string>을 할당
        }

        for (int i = 0; i < recipeList.Length; i++)      // 레시피 개수만큼
        {
            //Debug.Log(recipes[i].recipe.Count);     // 레시피 속에 사용되는 재료들의 개수
            for (int j = 0; j < recipes[i].recipe.Count; j++)       // 레시피SO 속에 레시피 리스트의 카운트 만큼.
            {
                recipeList[i].Add(recipes[i].recipe[j]);             // 지금 레시피 set에서 레시피SO 속에 레시피 j를 넣어줌.
            }
        }
    }

    public GameObject Interaction(GameObject ingredient = null)
    {
        if (interactive && (!one || !two) && !result)
        {
            //Debug.Log("재료합치기");
            playerInteraction.CurrentObjectInHand = null;       // 손에서 내려두고
            ingredient.transform.parent = this.transform;       // 부모 설정해주고
            if (!one)       // 첫번째에 재료가 없으면
            {
                one = true;
                ingredient.transform.position = pen[0].transform.position;
            }
            else if (one && !two)       // 두번째에 재료가 없으면
            {
                ingredient.transform.position = pen[1].transform.position;
                two = true;
            }
            playerInteraction.Is_Object = false;
            inputList.Add(ingredient.name);            // 이렇게 받아와주는데 후 수정가능하게? 이름 수정하기잉 
        }
        else if (interactive && (!one || !two) && result)       // 인터랙션 가능하고 조합이 완료되었으며 리솔츠가 있을 때
        {
            if (playerInteraction.CurrentObjectInHand == null) {
                //Debug.Log("완성품 가져갑니다!");
                playerInteraction.Is_Object = true;     // 오브젝트가 손에 들려있으니까.
                result = false;     // 가져갔으니까.
                return resultObject;
             }
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
            //Debug.Log(playerFOV.CheckForObjectsInView().name);
            if (playerFOV.CheckForObjectsInView().name == gameObject.transform.name)
            {
                if (!one || !two)
                {
                    playerInteraction.Is_Object = true;
                }
                interactive = true;
                // Debug.Log("플레이어 인터랙션 시 재료를 버려요.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInteraction.Is_Object = false;
            interactive = false;
        }
    }

    private void LateUpdate()
    {
        if (one && two)
        {
            //Debug.Log("레시피를 확인해서 결과가 나와용");
            // 레시피 확인해주기
            inputList.Sort();
            bool exist = false;
            for (int i = 0; i < recipes.Length; ++i)         // 레시피 개수(배열개수)만금 돌려주기
            {
                exist = false;

                recipeList[i].Sort();       // string 정렬해주기

                // 정렬해준거 끼리 확인하기
                for (int j = 0; j < recipeList[i].Count; j++)
                {
                    exist = true;
                    if (recipeList[i][j] != inputList[j])       // 다르면!
                    {
                        // 브레이크나 컨티뉴 잘 쓰깅
                        exist = false;
                        break;
                    }
                }

                //Debug.Log("확인");

                if (exist)
                {
                    //Debug.Log("아이템 생성됨");
                    //Debug.Log(recipes[i].weaponPrefab);
                    resultObject = Instantiate(recipes[i].weaponPrefab,  transform.position, Quaternion.identity);
                    //Debug.Log(resultObject);
                    resultObject.transform.parent = gameObject.transform;     // 내꺼에 자식으로 넣어줌.
                    resultObject.transform.localPosition = new Vector3(0, 0.5f, 0);        // 위치설정
                    break;
                }
            }
            if (!exist)
            {
                resultObject = Instantiate(garbagePrefab, transform.position, Quaternion.identity);
                resultObject.transform.parent = gameObject.transform;
                resultObject.transform.localPosition = new Vector3(0, 0.5f, 0);
            }

            int n = gameObject.transform.childCount;
            for (int i = n - 2; i >= 2; i--)
            {
                // 2보다 클 때까지
                // 오브젝트들 지워주기 풀링 나중에 사용하기!
                //Debug.Log(gameObject.transform.GetChild(i).name);
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }

            inputList.Clear();
            result = true;
            one = two = false;
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
