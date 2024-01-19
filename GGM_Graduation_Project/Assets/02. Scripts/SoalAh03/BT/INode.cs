public enum NodeState
{
    Success,
    Failure,
    Running
}

public interface INode
{
    void OnAwake();
    void OnStart();
    NodeState Execute();
    void OnEnd();
}