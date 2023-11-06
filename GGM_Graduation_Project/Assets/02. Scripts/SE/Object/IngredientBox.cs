using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class IngredientBox : MonoBehaviour, IObject
{
    [SerializeField] private GameObject giveIngredientPrefab;
    private bool interactive = false;

    private PlayerFOV playerFOV;
    private PlayerInteraction playerInteraction;

    public GameObject Interaction(GameObject ingredient = null)
    {
        if (interactive)
        {
            // ���ͷ��� �ڵ� �ۼ�
            // ������Ʈ Ǯ�� ����ϱ�?
            GameObject item = Instantiate(giveIngredientPrefab, transform.position, Quaternion.identity);
            item.name = item.name.Substring(0, item.name.IndexOf('('));
            return item;
        }
        return null; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("FŰ�� ��ȣ�ۿ��� �����ؿ�!");
            // ��ȣ�ۿ� ���� ǥ�����ֱ�
            if (playerFOV == null)
            {
                playerFOV = other.gameObject.GetComponent<PlayerFOV>();
                playerInteraction = other.gameObject.GetComponent<PlayerInteraction>();
            }
            if (playerFOV.CheckForObjectsInView() == gameObject.transform.name)
            {
                playerInteraction.is_GetIngredient = true;
                interactive = true;
                Debug.Log("�÷��̾� ���ͷ��� �� ��Ḧ �޾ƿ�");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //��ȣ�ۿ� ���� ǥ�����ֱ�
            playerInteraction.is_GetIngredient = false;
            interactive = false;
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

       // gameObject.GetComponent<IObject>().Interaction();