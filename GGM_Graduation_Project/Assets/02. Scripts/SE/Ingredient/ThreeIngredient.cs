using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeIngredient : Ingredient
{
    public ThreeIngredientType type = ThreeIngredientType.raw;
    public string name;

    private void Awake()
    {
        Init(type);
        gameObject.name = gameObject.name.Substring(0, gameObject.name.IndexOf('('));
        name = gameObject.name;
        gameObject.name = $"{name}-{System.Enum.GetName(typeof(ThreeIngredientType), type)}";
    }

    public void TypeChange()
    {
        if (ChangeType(type))
        {
            gameObject.name = $"{name}-{System.Enum.GetName(typeof(ThreeIngredientType), type)}";
            type = type + 1;
        }
    }
}
