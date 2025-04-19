using AdminToys;
using Interactables.Interobjects;
using JetBrains.Annotations;
using MapGeneration.Distributors;
using Qurre.API.Entities.AdminToys;
using Qurre.API.Entities.Characters;
using Qurre.API.Entities.Doors;
using Qurre.API.Entities.Environment;
using Qurre.API.Entities.Structures;
using Qurre.API.Enums;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class InteractDoorEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.InteractDoor;

    internal InteractDoorEvent(Player player, IDoor door, bool allowed)
    {
        Player = player;
        Door = door;
        Allowed = allowed;
    }

    public Player Player { get; }
    public IDoor Door { get; }
    public bool Allowed { get; set; }

    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class InteractGeneratorEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.InteractGenerator;

    internal InteractGeneratorEvent(Player player, IGenerator generator, GeneratorStatus status)
    {
        Player = player;
        Generator = generator;
        Status = status;
        Allowed = true;
    }

    public Player Player { get; }
    public IGenerator Generator { get; }
    public GeneratorStatus Status { get; }
    public bool Allowed { get; set; }

    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class InteractLiftEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.InteractLift;

    internal InteractLiftEvent(Player player, ILift lift)
    {
        Player = player;
        Lift = lift;
        Allowed = true;
    }

    public Player Player { get; }
    public ILift Lift { get; }
    public bool Allowed { get; set; }

    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class InteractLockerEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.InteractLocker;

    internal InteractLockerEvent(Player player, ILocker locker, LockerChamber? chamber, bool allow)
    {
        Player = player;
        Locker = locker;
        Chamber = chamber;
        Allowed = allow;
    }

    public Player Player { get; }
    public ILocker Locker { get; }
    public LockerChamber? Chamber { get; }
    public bool Allowed { get; set; }

    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class InteractScp330Event : IBaseEvent
{
    private const uint EventID = PlayerEvents.InteractScp330;

    internal InteractScp330Event(Player player, Scp330Interobject scp330)
    {
        Player = player;
        Scp330 = scp330;
        Allowed = true;
    }

    public Player Player { get; }
    public Scp330Interobject Scp330 { get; }
    public bool Allowed { get; set; }

    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class InteractShootingTargetEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.InteractShootingTarget;

    internal InteractShootingTargetEvent(Player player, IShootingTarget shootingTarget,
        ShootingTarget.TargetButton button)
    {
        Player = player;
        ShootingTarget = shootingTarget;
        Button = button;
        Allowed = true;
    }

    public Player Player { get; }
    public IShootingTarget ShootingTarget { get; }
    public ShootingTarget.TargetButton Button { get; set; }
    public bool Allowed { get; set; }

    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class InteractWorkStationEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.InteractWorkStation;

    internal InteractWorkStationEvent(Player player, IWorkStation station, byte colliderId)
    {
        Player = player;
        Station = station;
        ColliderId = colliderId;
        Allowed = true;
    }

    public Player Player { get; }
    public IWorkStation Station { get; }
    public byte ColliderId { get; }
    public bool Allowed { get; set; }

    public uint EventId { get; } = EventID;
}