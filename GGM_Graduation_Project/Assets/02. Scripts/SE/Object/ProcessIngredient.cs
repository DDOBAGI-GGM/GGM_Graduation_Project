using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class ProcessIngredient : MonoBehaviour, IObject
{
    [SerializeField] private float deleyTime;

    public GameObject Interaction(GameObject ingredient)        // �̰� Ȯ�����ֱ�
    {
        if (ingredient != null)
        {
            // ��ũ��Ʈ �޾ƿ��ֱ�
            //Debug.Log("���μ���");
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
            Debug.Log($"��ٸ��� ��... {i}/{deleyTime}");
        }
        //Ingredient next = ingredient.GetComponent<Ingredient>();
        ThreeIngredient three = ingredient.GetComponent<ThreeIngredient>();
        if (three != null)
        {
            Debug.Log("�� ���� ã�Ҿ��!");
            three.TypeChange();
        }
        else
        {
            TwoIngredient two = ingredient.GetComponent<TwoIngredient>();
            if (two != null)
            {
                Debug.Log("�� ���� ã�Ҿ��!");
                two.TypeChange();
            }
        }
    }
}
