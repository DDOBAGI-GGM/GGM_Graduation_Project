using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    enemyAttack = 0,
    objectAttack,
    floorAttack,
    Recovery            // ȸ���̶�� �߰�����.
}

[CreateAssetMenu(menuName = "SO/Recipes", fileName = "Recipes")]
public class RecipeListSO : ScriptableObject
{
    public GameObject weaponPrefab;
    public List<string> recipe = new List<string>();
    public WeaponType weaponType;           // �� �� ��������? ������ �� �̸��� �ƴ϶� �̰ɷ� �޾ƿ;� �� �� ����.
}

//public HashSet<string> recipe = new HashSet<string>();
//public SortedSet<string> recipe = new SortedSet<string>();
//public Dictionary<string, object> data;


// ���, ����, ����