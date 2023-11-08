using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    playerAttack = 0,
    objectAttack,
    floorAttack
}

[CreateAssetMenu(menuName = "SO/Recipes", fileName = "Recipes")]
public class RecipeListSO : ScriptableObject
{
    public GameObject weaponPrefab;
    public List<string> recipe = new List<string>();
    public WeaponType weaponType;
}

//public HashSet<string> recipe = new HashSet<string>();
//public SortedSet<string> recipe = new SortedSet<string>();
//public Dictionary<string, object> data;


// 사과, 딸기, 포도