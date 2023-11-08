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

    private List<string> inputList = new List<string> ();           // ���� ���� ����
    private List<string>[] recipeList;          // �����ǵ� 3����
    private bool one, two, result = false;      // �̷��� �ؼ� ������ֱ�?
    public bool Result { get { return result; } private set { } }

    private PlayerFOV playerFOV;
    private PlayerInteraction playerInteraction;

    [SerializeField] private Transform[] pen = new Transform[2];
    [SerializeField] private RecipeListSO[] recipes;
    [SerializeField] private GameObject garbagePrefab;
    private GameObject resultObject = null;        // �ϼ�ǰ�� �ִ���, ������ �÷��̾ ��ȣ�ۿ� �� �ϼ�ǰ�� �־����

    private void Awake()
    {
        recipeList = new List<string>[recipes.Length];       // ������SO ������ ����.
        //Debug.Log(recipeList.Length);

        for (int i = 0; i < recipeList.Length; i++)
        {
            recipeList[i] = new List<string>(); // �� ��ҿ� List<string>�� �Ҵ�
        }

        for (int i = 0; i < recipeList.Length; i++)      // ������ ������ŭ
        {
            //Debug.Log(recipes[i].recipe.Count);     // ������ �ӿ� ���Ǵ� ������ ����
            for (int j = 0; j < recipes[i].recipe.Count; j++)       // ������SO �ӿ� ������ ����Ʈ�� ī��Ʈ ��ŭ.
            {
                recipeList[i].Add(recipes[i].recipe[j]);             // ���� ������ set���� ������SO �ӿ� ������ j�� �־���.
            }
        }
    }

    public GameObject Interaction(GameObject ingredient = null)
    {
        if (interactive && (!one || !two) && !result)
        {
            //Debug.Log("�����ġ��");
            playerInteraction.CurrentObjectInHand = null;       // �տ��� �����ΰ�
            ingredient.transform.parent = this.transform;       // �θ� �������ְ�
            if (!one)       // ù��°�� ��ᰡ ������
            {
                one = true;
                ingredient.transform.position = pen[0].transform.position;
            }
            else if (one && !two)       // �ι�°�� ��ᰡ ������
            {
                ingredient.transform.position = pen[1].transform.position;
                two = true;
            }
            playerInteraction.Is_Object = false;
            inputList.Add(ingredient.name);            // �̷��� �޾ƿ��ִµ� �� ���������ϰ�? �̸� �����ϱ��� 
        }
        else if (interactive && (!one || !two) && result)       // ���ͷ��� �����ϰ� ������ �Ϸ�Ǿ����� �������� ���� ��
        {
            if (playerInteraction.CurrentObjectInHand == null) {
                //Debug.Log("�ϼ�ǰ �������ϴ�!");
                playerInteraction.Is_Object = true;     // ������Ʈ�� �տ� ��������ϱ�.
                result = false;     // ���������ϱ�.
                return resultObject;
             }
        }
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("FŰ�� ��ȣ�ۿ��� �����ؿ�!");
            // ��ȣ�ۿ� ���� ǥ�����ֱ�
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
                // Debug.Log("�÷��̾� ���ͷ��� �� ��Ḧ ������.");
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
            //Debug.Log("�����Ǹ� Ȯ���ؼ� ����� ���Ϳ�");
            // ������ Ȯ�����ֱ�
            inputList.Sort();
            bool exist = false;
            for (int i = 0; i < recipes.Length; ++i)         // ������ ����(�迭����)���� �����ֱ�
            {
                exist = false;

                recipeList[i].Sort();       // string �������ֱ�

                // �������ذ� ���� Ȯ���ϱ�
                for (int j = 0; j < recipeList[i].Count; j++)
                {
                    exist = true;
                    if (recipeList[i][j] != inputList[j])       // �ٸ���!
                    {
                        // �극��ũ�� ��Ƽ�� �� ����
                        exist = false;
                        break;
                    }
                }

                //Debug.Log("Ȯ��");

                if (exist)
                {
                    //Debug.Log("������ ������");
                    //Debug.Log(recipes[i].weaponPrefab);
                    resultObject = Instantiate(recipes[i].weaponPrefab,  transform.position, Quaternion.identity);
                    //Debug.Log(resultObject);
                    resultObject.transform.parent = gameObject.transform;     // ������ �ڽ����� �־���.
                    resultObject.transform.localPosition = new Vector3(0, 0.5f, 0);        // ��ġ����
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
                // 2���� Ŭ ������
                // ������Ʈ�� �����ֱ� Ǯ�� ���߿� ����ϱ�!
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
