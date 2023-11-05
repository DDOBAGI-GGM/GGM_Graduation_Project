using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Ingredient
{
    private ThreeIngredientType type;

    private void Awake()
    {
        Init(type);
    }
}
