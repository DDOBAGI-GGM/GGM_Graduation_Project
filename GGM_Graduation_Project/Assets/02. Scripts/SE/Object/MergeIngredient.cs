using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MergeIngredient : MonoBehaviour, IObject
{
    private bool interactive = false;

    private SortedSet<string> inputs = new SortedSet<string>();            // ���� ���� ��� ����
    private SortedSet<string>[] recipeSet;      // ��� ����
    private bool one, two;      // �̷��� �ؼ� ������ֱ�?

    private PlayerFOV playerFOV;
    private PlayerInteraction playerInteraction;

    [SerializeField] private Transform[] pen = new Transform[2];
    [SerializeField] private RecipeListSO[] recipes;

    private void Awake()
    {
        recipeSet = new SortedSet<string>[recipes.Length];       // ������SO ������ ����.
        for (int i = 0; i < recipeSet.Length; i++)      // ������Set ������ŭ
        {
            for (int j = 0; j < recipes[i].recipe.Count; j++)       // ������SO �ӿ� ������ ����Ʈ�� ī��Ʈ ��ŭ.
            {
                recipeSet[i].Add(recipes[i].recipe[j]);             // ���� ������ set���� ������SO �ӿ� ������ j�� �־���.
            }
        }
    }

    public GameObject Interaction(GameObject ingredient)
    {
        if (interactive && (!one || !two))
        {
            Debug.Log("�����ġ��");
            playerInteraction.CurrentObjectInHand = null;       // �տ��� �����ΰ�
            ingredient.transform.parent = this.transform;       // �θ� �������ְ�
            if (!one)       // ù��°�� ��ᰡ ������
            {
                one = true;
                ingredient.transform.position = pen[0].transform.position;
            }
            else if (one && !two)       // �ι�°�� ��ᰡ ������
            {
                two = true;
                ingredient.transform.position = pen[1].transform.position;
            }
            inputs.Add(ingredient.name);            // �̷��� �޾ƿ��ִµ� �� ���������ϰ�? �̸� �����ϱ��� 
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
            if (playerFOV.CheckForObjectsInView().name == gameObject.transform.name)
            {
                interactive = true;
                // Debug.Log("�÷��̾� ���ͷ��� �� ��Ḧ ������.");
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
            // ������ Ȯ�����ֱ�
            foreach (SortedSet<string> recipe in recipeSet)
            {

            }


            one = two = false;      // �Ѵ� �޽�
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
