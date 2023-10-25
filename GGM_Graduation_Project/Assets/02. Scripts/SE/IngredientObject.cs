using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : Object
{
    [SerializeField] private GameObject ingredientPrefab;

    private Vector3 createPos;

    private void Awake()
    {
        createPos = transform.position;
    }

    public GameObject GetIngredient()
    {
        GameObject ingredient = Instantiate(ingredientPrefab, createPos, Quaternion.identity);
        return ingredient;
    }

    public override void Interaction(Ingredient type)
    {
        Delay(3f);
        type.Type = type.Type + 1;
    }
}