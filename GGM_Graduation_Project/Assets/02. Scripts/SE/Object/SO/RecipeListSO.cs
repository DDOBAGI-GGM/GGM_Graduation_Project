using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    enemyAttack = 0,
    objectAttack,
    floorAttack,
    Recovery            // 회복이라고 추가해줌.
}

[CreateAssetMenu(menuName = "SO/Recipes", fileName = "Recipes")]
public class RecipeListSO : ScriptableObject
{
    public GameObject weaponPrefab;
    public List<string> recipe = new List<string>();
    public WeaponType weaponType;           // 이 걸 언제쓸까? 공격할 때 이름이 아니라 이걸로 받아와야 할 것 같음.
}

//public HashSet<string> recipe = new HashSet<string>();
//public SortedSet<string> recipe = new SortedSet<string>();
//public Dictionary<string, object> data;


// 사과, 딸기, 포도