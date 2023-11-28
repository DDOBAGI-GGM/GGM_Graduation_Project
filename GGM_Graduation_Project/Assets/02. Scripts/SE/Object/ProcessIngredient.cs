using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class ProcessIngredient : MonoBehaviour, IObject
{
    [SerializeField] private float deleyTime;

    public GameObject Interaction(GameObject ingredient)        // 이고 확인해주기
    {
        if (ingredient != null)
        {
            // 스크립트 받아와주기
            //Debug.Log("프로세싱");
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
}
