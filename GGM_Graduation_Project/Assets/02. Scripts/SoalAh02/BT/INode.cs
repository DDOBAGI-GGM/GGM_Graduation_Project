public enum NodeState
{
    Success,
    Failure,
    Running
}

public interface INode
{
    NodeState Execute();
}