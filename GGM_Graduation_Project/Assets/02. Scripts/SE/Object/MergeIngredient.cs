using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MergeIngredient : MonoBehaviour, IObject
{
    private List<string> inputList = new List<string> ();           // ���� ���� ����
    private List<string>[] recipeList;          // �����ǵ� 3����
    private bool one, two, result = false;      // �̷��� �ؼ� ������ֱ�?
    public bool Result { get { return result; } private set { } }

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
        if (((!one || !two) && !result) && ingredient)
        {
            //Debug.Log("�����ġ��");
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
            inputList.Add(ingredient.name);            // �̷��� �޾ƿ��ִµ� �� ���������ϰ�? �̸� �����ϱ��� 
        }
        else if (((!one || !two) && result) && ingredient == null)       // ���ͷ��� �����ϰ� ������ �Ϸ�Ǿ����� �������� �ְ� �Ű������� ���� ���� ���� ��
        {
           /* if (playerInteraction.CurrentObjectInHand == null) {
                //Debug.Log("�ϼ�ǰ �������ϴ�!");
                playerInteraction.Is_Object = true;     // ������Ʈ�� �տ� ��������ϱ�.*/
                result = false;     // ���������ϱ�.
                return resultObject;
             //}
        }
        return null;
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
}



// ���� ���� : ������ SO�� �����ǵ�(string �迭)�� �������ְ� ���� �͵��� ��� �̸� ���� �����ؼ� ���� Ȯ������.
// �׷��� ������ �ش� �������� �ϼ� ǰ�� ������.