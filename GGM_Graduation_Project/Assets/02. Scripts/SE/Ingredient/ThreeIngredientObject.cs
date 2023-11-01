using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeIngredientObject : Ingredient<ThreeIngredientType>
{
    [SerializeField] private GameObject ingredientPrefab;

    private void Awake()
    {
        base.Awake();
    }
}
