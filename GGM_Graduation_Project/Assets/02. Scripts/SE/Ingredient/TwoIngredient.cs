using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoIngredient : Ingredient
{
    public TwoIngredientType type = TwoIngredientType.raw;
    public string name;

    private void Awake()
    {
        Init(type);
        //gameObject.name = gameObject.name.Substring(0, gameObject.name.IndexOf('('));
        name = gameObject.name;
        gameObject.name = $"{name}_{System.Enum.GetName(typeof(TwoIngredientType), type)}";
    }

    public void TypeChange()
    {
        if (ChangeType(type))
        {
            gameObject.name = $"{name}_{System.Enum.GetName(typeof(TwoIngredientType), type + 1)}";
            type = type + 1;
        }
    }
}
