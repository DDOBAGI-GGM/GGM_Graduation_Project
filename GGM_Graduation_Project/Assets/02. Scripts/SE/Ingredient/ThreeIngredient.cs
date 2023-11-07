using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeIngredient : Ingredient
{
    public ThreeIngredientType type = ThreeIngredientType.raw;

    private void Awake()
    {
        Init(type);
    }

    public void TypeChange()
    {
        if (ChangeType(type))
        {
            type = type + 1;
        }
    }
}
