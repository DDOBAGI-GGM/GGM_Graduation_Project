using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateType
{
    None,
    Ingredient = 0,
    Processing = 1,
    Merge = 2,
    Attack = 3,
    Shelf = 4,
    Trash = 5 ,
}

[System.Serializable]
public class RECIPE
{
    public RecipeListSO recipe;
    public int index;
}

[System.Serializable]
public class RECIPE_Dev
{
    public RECIPE recipe;
    public RECIPE oldRecipe;
    public int probability;
    public bool available;
}

[System.Serializable]
public class OBJ
{
    public string name;
    public List<ITEM> obj = new List<ITEM>();
}

[System.Serializable]
public class ITEM
{
    public string name;
    public GameObject item;
}

public class AIManager : MonoBehaviour
{
    // ������
    public List<RECIPE_Dev> recipes = new List<RECIPE_Dev>();

    // ������Ʈ
    public List<OBJ> objects = new List<OBJ>();

    private void Awake()
    {
    }

    private void Start()
    {
    }
}
