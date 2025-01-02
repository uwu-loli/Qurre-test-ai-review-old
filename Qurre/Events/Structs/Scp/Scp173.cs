using JetBrains.Annotations;
using Qurre.API.Controllers;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class Scp173AddObserverEvent : IBaseEvent
{
    private const uint EventID = ScpEvents.Scp173AddObserver;

    internal Scp173AddObserverEvent(Player pl, Player scp)
    {
        Player = pl;
        Scp = scp;
        Allowed = true;
    }

    public Player Player { get; }
    public Player Scp { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class Scp173RemovedObserverEvent : IBaseEvent
{
    private const uint EventID = ScpEvents.Scp173RemovedObserver;

    internal Scp173RemovedObserverEvent(Player pl, Player scp)
    {
        Player = pl;
        Scp = scp;
    }

    public Player Player { get; }
    public Player Scp { get; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class Scp173EnableSpeedEvent : IBaseEvent
{
    private const uint EventID = ScpEvents.Scp173EnableSpeed;

    internal Scp173EnableSpeedEvent(Player pl, bool value)
    {
        Player = pl;
        Value = value;
        Allowed = true;
    }

    public Player Player { get; }
    public bool Value { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}