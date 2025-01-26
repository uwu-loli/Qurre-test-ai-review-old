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

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
    
    public Player Player { get; }
    public ITesla Tesla { get; }
    public bool InIdlingRange { get; }
    public bool InRageRange { get; set; }
    
    internal TriggerTeslaEvent(Player player, ITesla tesla, bool inIdlingRange, bool inRageRange)
    {
        Player = player;
        Tesla = tesla;
        InIdlingRange = inIdlingRange;
        InRageRange = inRageRange;
    }
}

[PublicAPI]
public class WorkStationUpdateEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.WorkStationUpdate;

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
    
    public IWorkStation Station { get; }
    
    internal WorkStationUpdateEvent(IWorkStation station)
    {
        Station = station;
    }
}

[PublicAPI]
public class IntercomSetStateEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.IntercomSetState;

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
    
    public IntercomState State { get; set; }
    
    internal IntercomSetStateEvent(IntercomState state)
    {
        State = state;
    }
}