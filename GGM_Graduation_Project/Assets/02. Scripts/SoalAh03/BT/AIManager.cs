using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateType
{
    Ingredient = 0,
    Processing = 1,
    Merge = 2,
    Attack = 3,
    Shelf = 4,
    Trash = 5 ,
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

[System.Serializable]
public class RECIPE
{
    public RecipeListSO recipe;
    //public int percent
}

public class AIManager : MonoBehaviour
{
    // 레시피
    public List<RecipeListSO> recipes = new List<RecipeListSO>();

    // 오브젝트
    public List<OBJ> objects = new List<OBJ>();

    //public SerializableDictionary<string, GameObject> dictionary = new SerializableDictionary<string, GameObject>();

    private void Awake()
    {
    }

    private void Start()
    {
    }
}
