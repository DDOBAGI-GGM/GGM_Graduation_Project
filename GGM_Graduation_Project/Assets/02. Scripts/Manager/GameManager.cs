using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float aiHp;
    public float playerHp;

    [SerializeField] private float maxHp;

    private void Start()
    {
        aiHp = playerHp = maxHp;
    }

    private void SetHP()
    {

    }
}
