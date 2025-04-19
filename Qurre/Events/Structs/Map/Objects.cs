using JetBrains.Annotations;
using PlayerRoles.Voice;
using Qurre.API.Entities.Characters;
using Qurre.API.Entities.Environment;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class TriggerTeslaEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.TriggerTesla;

    internal TriggerTeslaEvent(Player player, ITesla tesla, bool inIdlingRange, bool inRageRange)
    {
        Player = player;
        Tesla = tesla;
        InIdlingRange = inIdlingRange;
        InRageRange = inRageRange;
    }

    public Player Player { get; }
    public ITesla Tesla { get; }
    public bool InIdlingRange { get; }
    public bool InRageRange { get; set; }

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
}

[PublicAPI]
public class WorkStationUpdateEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.WorkStationUpdate;

    internal WorkStationUpdateEvent(IWorkStation station)
    {
        Station = station;
    }

    public IWorkStation Station { get; }

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
}

[PublicAPI]
public class IntercomSetStateEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.IntercomSetState;

    internal IntercomSetStateEvent(IntercomState state)
    {
        State = state;
    }

    public IntercomState State { get; set; }

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
}