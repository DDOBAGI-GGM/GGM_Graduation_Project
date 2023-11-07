using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class GarbageCan : MonoBehaviour, IObject
{
    private bool interactive = false;

    private PlayerFOV playerFOV;
    private PlayerInteraction playerInteraction;

    public GameObject Interaction(GameObject ingredient)
    {
        Debug.Log("qjfud");
        if (interactive)
        {
            //풀링 사용해서 해주기
            Debug.Log("버려주기");
            playerInteraction.CurrentObjectInHand = null;       // 손에서 버려주기
            Destroy(ingredient);
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
                playerInteraction.Is_Object = true;
                interactive= true;
               // Debug.Log("플레이어 인터랙션 시 재료를 버려요.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //상호작용 가능 표시해주기
            playerInteraction.Is_Object = false;
            interactive= false;
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
