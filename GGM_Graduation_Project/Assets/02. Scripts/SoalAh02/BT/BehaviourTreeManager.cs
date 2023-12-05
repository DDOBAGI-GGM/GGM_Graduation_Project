using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeManager<T> where T : INode
{
    private T root;

    public void SetRoot(T rootNode)
    {
        root = rootNode;
    }

    public void Update()
    {
        root?.Execute();
    }
}