using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBox : MonoBehaviour, IObject
{
    [SerializeField] private GameObject giveIngredientPrefab;

    public void Interaction(GameObject ingredient = null)
    {
        // ������Ʈ Ǯ�� ����ϱ�?
        Instantiate(giveIngredientPrefab, transform.position, Quaternion.identity);
    }
}

       // gameObject.GetComponent<IObject>().Interaction();