using BehaviorTree;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Brain : MonoBehaviour
{
    private BT_Node _curNode;
    [SerializeField] List<GameObject> _nodes = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
            _nodes.Add(transform.GetChild(i).gameObject);

        Init();
    }

    private void Init()
    {
        foreach (GameObject node in _nodes)
        {
            // 하나로 다 묶고 싶은데...
        }
    }
}
