using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IngredientBox : MonoBehaviour, IObject
{
    [SerializeField] private GameObject giveIngredientPrefab;
    public GameObject Interaction(GameObject ingredient = null)
    {
        GameObject item = Instantiate(giveIngredientPrefab, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySFX("get");
        return item;
    }
}