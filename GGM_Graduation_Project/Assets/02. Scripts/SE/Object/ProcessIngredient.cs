using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessIngredient : MonoBehaviour, IObject
{
    public void Interaction(GameObject Ingredient)
    {
        // ��ũ��Ʈ �޾ƿ��ֱ�

        StopCoroutine(InteractionRoutine(2f));
        StartCoroutine(InteractionRoutine(2f));
    }

    public IEnumerator InteractionRoutine(float time)
    {
       yield return new WaitForSeconds(time);
        /*
        if (threeIngredient != null)
        {
            threeIngredient.Type = threeIngredient.Type + 1;
        }
        else if (twoIngredient != null)
        {
            twoIngredient.Type = twoIngredient.Type + 1;
        }*/
    }
}
