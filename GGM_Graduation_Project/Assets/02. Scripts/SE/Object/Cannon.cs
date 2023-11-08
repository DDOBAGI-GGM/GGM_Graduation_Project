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
        // 상대편을 비추고, 발사를 누르면 상대편으로 날라감.
        if (interactive)
        {
            ingredient.transform.parent = transform;            // 자식으로 넣을까 안넣을까
            ingredient.transform.localPosition = new Vector3(0, 0.5f, 0);
            ingredient.transform.parent = null;
            playerInteraction.CurrentObjectInHand = null;
            //playerInteraction.Is_Object = false;
            Debug.Log("무기발사 가보자고!");

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
                    Debug.Log("올바른 무기 유형이 아니여서 터졌어요!");
                    // 풀링사용하기?
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
            //Debug.Log("F키로 상호작용이 가능해요!");
            // 상호작용 가능 표시해주기
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
            //상호작용 가능 표시해주기
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
