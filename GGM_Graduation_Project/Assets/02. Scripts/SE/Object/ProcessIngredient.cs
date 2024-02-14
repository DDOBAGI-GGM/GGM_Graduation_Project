using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(SphereCollider))]
public class ProcessIngredient : MonoBehaviour, IObject
{
    [SerializeField] private float deleyTime;
    [SerializeField] private Slider deleySlider;

    private Vector3 playerTrm;

    private void Awake()
    {
    }

    public GameObject Interaction(GameObject ingredient)        // �̰� Ȯ�����ֱ�
    {
        if (ingredient != null)
        {
            // ��ũ��Ʈ �޾ƿ��ֱ�
            //Debug.Log(ingredient.transform.position);
            //Debug.Log(playerTrm);
            deleySlider.gameObject.SetActive(true);
            playerTrm = ingredient.transform.position;
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
            Tween t = DOTween.To(() => deleySlider.value, value => deleySlider.value  = value,  i / deleyTime, 1f);
            t.Play();
            //deleySlider.value = i / deleyTime;
            yield return time;
            if (ingredient.transform.position != playerTrm)
            {
                // �� �������� �۾���...
                deleySlider.value = 0;
                deleySlider.gameObject.SetActive(false);
                StopCoroutine(InteractionRoutine(ingredient));
                yield break;        // ��������.
            }
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
        deleySlider.gameObject.SetActive(false);
        deleySlider.value = 0;
    }
}
