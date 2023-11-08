using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneIngredient : MonoBehaviour
{
    public string name;

    private void Awake()
    {
        gameObject.name = gameObject.name.Substring(0, gameObject.name.IndexOf('('));
        name = gameObject.name;
    }
}
