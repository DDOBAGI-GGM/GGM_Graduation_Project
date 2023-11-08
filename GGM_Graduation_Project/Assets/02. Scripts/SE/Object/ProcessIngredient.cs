using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ProcessIngredient : MonoBehaviour, IObject
{
    private bool interactive = false;

    private PlayerFOV playerFOV;
    private PlayerInteraction playerInteraction;

    [SerializeField] private float deleyTime;

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
//                Debug.Log("플레이어 인터랙션 시 재료의 프로세싱이 시작되요.");
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

    public GameObject Interaction(GameObject ingredient)        // 이고 확인해주기
    {
        if (interactive)
        {
            // 스크립트 받아와주기
            Debug.Log("프로세싱");
            StopCoroutine(InteractionRoutine(ingredient));
            StartCoroutine(InteractionRoutine(ingredient));
        }
        return null;
    }

    public IEnumerator InteractionRoutine(GameObject ingredient)
    {
        var time = new WaitForSeconds(1f);
        for (int i = 1; i <= deleyTime; i++)
        {
            yield return time;
            Debug.Log($"기다리는 중... {i}/{deleyTime}");
        }
        //Ingredient next = ingredient.GetComponent<Ingredient>();
        ThreeIngredient three = ingredient.GetComponent<ThreeIngredient>();
        if (three != null)
        {
            Debug.Log("세 개를 찾았어요!");
            three.TypeChange();
        }
        else
        {
            TwoIngredient two = ingredient.GetComponent<TwoIngredient>();
            if (two != null)
            {
                Debug.Log("두 개를 찾았어요!");
                two.TypeChange();
            }
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
