using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    // 현재 조리법
    [SerializeField] private RecipeListSO curRecipe;

    // 손
    [Header("AI")]
    [SerializeField] GameObject hand;

    // 정보
    //[Header("Intelligence")]
    private float speed;
}
