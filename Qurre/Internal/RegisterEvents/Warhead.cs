using LabApi.Events.Handlers;
using Qurre.API;
using Qurre.Events.Structs;
using Qurre.Internal.Attributes;
using Qurre.Internal.EventsManager;
using LabApiEvents = LabApi.Events.Arguments.WarheadEvents;

namespace Qurre.Internal.RegisterEvents;

internal static class Warhead
{
    [SelfInvoke]
    internal static void Init()
    {
        WarheadEvents.Starting += OnStart;
        WarheadEvents.Stopping += OnStop;
        WarheadEvents.Detonated += OnDetonate;
    }

    private static void OnStart(LabApiEvents.WarheadStartingEventArgs ev)
    {
        AlphaStartEvent @event = new(ev.Player.GetPlayer(), ev.IsAutomatic, ev.SuppressSubtitles, ev.WarheadState);
        @event.InvokeEvent();

        ev.IsAutomatic = @event.Automatic;
        ev.IsAllowed = @event.Allowed;
        ev.SuppressSubtitles = @event.SuppressSubtitles;
        ev.WarheadState = @event.State;
    }

    private static void OnStop(LabApiEvents.WarheadStoppingEventArgs ev)
    {
        AlphaStopEvent @event = new(ev.Player.GetPlayer(), ev.WarheadState);
        @event.InvokeEvent();

        ev.WarheadState = @event.State;
        ev.IsAllowed = @event.Allowed;
    }

    private static void OnDetonate(LabApiEvents.WarheadDetonatedEventArgs ev)
    {
        AlphaDetonateEvent @event = new();
        @event.InvokeEvent();
    }
}