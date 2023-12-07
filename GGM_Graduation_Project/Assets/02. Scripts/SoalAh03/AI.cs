using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [Header("AI")]
    // 손
    public GameObject hand = null;
    public Transform handPos;
    // 현재 조리법
    public RecipeListSO recipe;
    // 속도
    public float speed;

    [Header("Intelligence")]
    public List<RecipeListSO> recipes;
    //public Dictionary<string, List<GameObject>> objects;
    public List<GameObject> objects;
}
