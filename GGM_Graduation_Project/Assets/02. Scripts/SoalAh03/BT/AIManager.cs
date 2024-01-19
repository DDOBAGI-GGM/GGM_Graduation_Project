using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateType
{
    None = 0,
    ingredient = 1,
    processing = 2,
    merge = 3,
    shelf = 4,
    attack = 5,
}

[System.Serializable]
public class OBJ
{
    public string name;
    public List<GameObject> obj = new List<GameObject>();
}

public class AIManager : MonoBehaviour
{
    // ������
    public List<GameObject> recipes = new List<GameObject>();

    // ������Ʈ
    public List<OBJ> objects = new List<OBJ>();

    public SerializableDictionary<string, GameObject> dictionary = new SerializableDictionary<string, GameObject>();

    private void Awake()
    {
    }

    private void Start()
    {
    }
}
