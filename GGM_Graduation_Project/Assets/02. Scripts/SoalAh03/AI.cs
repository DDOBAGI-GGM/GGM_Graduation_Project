using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [Header("AI")]
    // ��
    public GameObject hand = null;
    public Transform handPos;
    // ���� ������
    public RecipeListSO recipe;
    // �ӵ�
    public float speed;

    [Header("Intelligence")]
    public List<RecipeListSO> recipes;
    //public Dictionary<string, List<GameObject>> objects;
    public List<GameObject> objects;
}
