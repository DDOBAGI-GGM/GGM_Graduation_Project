using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class LogNode : INode
{
    private string text;
    private bool logError;
    private bool logTime;

    public LogNode(string text, bool logError = false, bool logTime = false)
    {
        this.text = text;
        this.logError = logError;
        this.logTime = logTime;
    }

    public void OnAwake()
    {
    }

    public void OnStart()
    {
    }

    public NodeState Execute()
    {
        if (logError)
        {
            Debug.LogError(logTime ? string.Format("{0}: {1}", Time.time, text) : text);
        }
        else
        {
            Debug.Log(logTime ? string.Format("{0}: {1}", Time.time, text) : text);
        }
        return NodeState.Success;
    }

    public void OnEnd()
    {
    }
}
