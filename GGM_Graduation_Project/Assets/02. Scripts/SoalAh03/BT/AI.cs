using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public AIManager manager;
    BehaviourTreeManager<INode> behaviourTreeManager;

    private void Awake()
    {
        manager = GameObject.Find("AIMgr").GetComponent<AIManager>();
    }

    void Start()
    {
        behaviourTreeManager = new BehaviourTreeManager<INode>();
        behaviourTreeManager.SetRoot(new SequenceNode(
            new SequenceNode(
                new WaitNode(5f),
                new LogNode("tt", false, true)
            )
        ));
    }

    void Update()
    {
        behaviourTreeManager.Update();
    }
}
