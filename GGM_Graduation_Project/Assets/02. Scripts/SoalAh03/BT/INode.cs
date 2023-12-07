public enum TNodeState
{
    Success,
    Failure,
    Running
}

public interface INode
{
    TNodeState Execute();
}