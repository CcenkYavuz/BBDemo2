using UnityEngine;

public interface IEvent { }

public struct AddTarget : IEvent {
    public GameObject target;
}
public struct RemoveTarget : IEvent
{
    public GameObject target;
}
public struct ChooseTarget : IEvent
{}