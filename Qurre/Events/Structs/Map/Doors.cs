using Interactables.Interobjects.DoorUtils;
using JetBrains.Annotations;
using Qurre.API.Entities.Doors;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class DamageDoorEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.DamageDoor;

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;

    public IBreakableDoor Door { get; }
    public DoorDamageType Type { get; }
    public float Damage { get; set; }
    
    internal DamageDoorEvent(IBreakableDoor door, DoorDamageType type, float damage)
    {
        Door = door;
        Type = type;
        Damage = damage;
    }
}

[PublicAPI]
public class LockDoorEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.LockDoor;

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
    
    public IDoor Door { get; }
    public DoorLockReason Reason { get; }
    public bool NewState { get; set; }
    
    internal LockDoorEvent(IDoor door, DoorLockReason reason, bool newState)
    {
        Door = door;
        Reason = reason;
        NewState = newState;
    }
}

[PublicAPI]
public class OpenDoorEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.OpenDoor;

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
    
    public IDoor Door { get; }
    public DoorEventOpenerExtension.OpenerEventType Type { get; }
    
    internal OpenDoorEvent(IDoor door, DoorEventOpenerExtension.OpenerEventType type)
    {
        Door = door;
        Type = type;
    }
}