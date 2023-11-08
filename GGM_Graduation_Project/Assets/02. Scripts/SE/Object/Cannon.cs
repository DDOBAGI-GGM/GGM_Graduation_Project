using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Cannon : MonoBehaviour, IObject
{
    private bool interactive = false;

    private PlayerFOV playerFOV;
    private PlayerInteraction playerInteraction;

    [SerializeField] private Transform floorPos;
    [SerializeField] private Transform enemyPos;
    [SerializeField] private Transform objectPos;

    public GameObject Interaction(GameObject ingredient)
    {
        // ������� ���߰�, �߻縦 ������ ��������� ����.
        if (interactive)
        {
            ingredient.transform.parent = transform;            // �ڽ����� ������ �ȳ�����
            ingredient.transform.localPosition = new Vector3(0, 0.5f, 0);
            ingredient.transform.parent = null;
            playerInteraction.CurrentObjectInHand = null;
            //playerInteraction.Is_Object = false;
            Debug.Log("����߻� �����ڰ�!");

            string type = ingredient.gameObject.name.Substring(ingredient.gameObject.name.IndexOf('-') + 1);
            Debug.Log(type);
            switch (type)
            {
                case "Floor":
                    Attack(ingredient, floorPos.position);
                    break;
                case "Object":
                    Attack(ingredient, objectPos.position);
                    break;
                case "Enemy":
                    Attack(ingredient, enemyPos.position);
                    break;
                default:
                    Debug.Log("�ùٸ� ���� ������ �ƴϿ��� �������!");
                    // Ǯ������ϱ�?
                    Destroy(ingredient);
                    break;
            }
        }
        return null;
    }

    private void Attack(GameObject weapon, Vector3 pos)
    {
        weapon.transform.DOMove(pos, 2f);
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
                playerInteraction.Is_Object = true;
                interactive = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //��ȣ�ۿ� ���� ǥ�����ֱ�
            playerInteraction.Is_Object = false;
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
