using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoIngredientObject : Ingredient<TwoIngredientType>
{
    [SerializeField] private GameObject ingredientPrefab;

    private void Awake()
    {
        base.Awake();
    }
}
