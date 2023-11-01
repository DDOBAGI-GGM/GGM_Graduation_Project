using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBox : MonoBehaviour, IObject
{
    [SerializeField] private GameObject giveIngredientPrefab;

    public void Interaction(GameObject ingredient = null)
    {
        // 오브젝트 풀링 사용하기?
        Instantiate(giveIngredientPrefab, transform.position, Quaternion.identity);
    }
}

       // gameObject.GetComponent<IObject>().Interaction();