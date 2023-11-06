using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MergeIngredient : MonoBehaviour, IObject
{
    private bool interactive = false;

    private PlayerFOV playerFOV;
    private PlayerInteraction playerInteraction;

    [SerializeField] private Transform[] pen = new Transform[2];

    public GameObject Interaction(GameObject ingredient = null)
    {
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
