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
            // 인터랙션 코드 작성
            // 오브젝트 풀링 사용하기?
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
            Debug.Log("F키로 상호작용이 가능해요!");
            // 상호작용 가능 표시해주기
            if (playerFOV == null)
            {
                playerFOV = other.gameObject.GetComponent<PlayerFOV>();
                playerInteraction = other.gameObject.GetComponent<PlayerInteraction>();
            }
            if (playerFOV.CheckForObjectsInView() == gameObject.transform.name)
            {
                playerInteraction.is_GetIngredient = true;
                interactive = true;
                Debug.Log("플레이어 인터랙션 시 재료를 받아요");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //상호작용 가능 표시해주기
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