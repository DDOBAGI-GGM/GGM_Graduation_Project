using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeManager<T> where T : INode
{
    private T root;

    public void SetRoot(T rootNode)
    {
        if (root != null)
            root.OnEnd();

        root = rootNode;
        root.OnAwake();
    }

    public void Update()
    {
        root?.Execute();
    }
}