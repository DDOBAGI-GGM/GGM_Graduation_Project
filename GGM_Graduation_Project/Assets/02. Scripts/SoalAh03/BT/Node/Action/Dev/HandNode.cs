using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text.RegularExpressions;

public class HandNode : INode
{
    AI ai;

    public HandNode(AI ai)
    {
        this.ai = ai;
    }

    public void OnAwake()
    {
    }

    public void OnStart()
    {
    }

    public NodeState Execute()
    {
        if (ai.hand)
        {
            string str = t(ai.hand.name);

            switch (str)
            {
                case "igredient":
                    break;
            }
        }
        else
        {
            ai.state = -1;
        }

        return NodeState.Success;
    }

    string t(string str)
    {
        Match match = Regex.Match(str, "-");
        return match.Value;
    }

    public void OnEnd()
    {
    }
}
