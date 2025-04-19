using Interactables.Interobjects.DoorUtils;
using JetBrains.Annotations;
using Qurre.API.Entities.Doors;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class DamageDoorEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.DamageDoor;

    internal DamageDoorEvent(IBreakableDoor door, DoorDamageType type, float damage)
    {
        Door = door;
        Type = type;
        Damage = damage;
    }

    public IBreakableDoor Door { get; }
    public DoorDamageType Type { get; }
    public float Damage { get; set; }

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
}

[PublicAPI]
public class LockDoorEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.LockDoor;

    internal LockDoorEvent(IDoor door, DoorLockReason reason, bool newState)
    {
        Door = door;
        Reason = reason;
        NewState = newState;
    }

    public IDoor Door { get; }
    public DoorLockReason Reason { get; }
    public bool NewState { get; set; }

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
}

[PublicAPI]
public class OpenDoorEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.OpenDoor;

    internal OpenDoorEvent(IDoor door, DoorEventOpenerExtension.OpenerEventType type)
    {
        Door = door;
        Type = type;
    }

    public IDoor Door { get; }
    public DoorEventOpenerExtension.OpenerEventType Type { get; }

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
}