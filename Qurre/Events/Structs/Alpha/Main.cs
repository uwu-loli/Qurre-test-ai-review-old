using JetBrains.Annotations;
using Qurre.API;
using Qurre.API.Entities.Characters;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

/// <summary>
///     Event triggered when an Alpha warhead start sequence is initiated
/// </summary>
[PublicAPI]
public sealed class AlphaStartEvent : IBaseEvent
{
    private const uint EventID = AlphaEvents.Start;

    internal AlphaStartEvent(
        Player? player,
        bool automatic,
        bool suppressSubtitles,
        AlphaWarheadSyncInfo state)
    {
        Player = player ?? Server.Host;
        Automatic = automatic;
        SuppressSubtitles = suppressSubtitles;
        State = state;
        Allowed = true;
    }

    /// <summary>
    ///     Gets the player who initiated the warhead sequence
    /// </summary>
    public Player Player { get; }

    /// <summary>
    ///     Gets or sets whether the sequence was automatically triggered
    /// </summary>
    public bool Automatic { get; set; }

    /// <summary>
    ///     Gets or sets whether to suppress CASSIE subtitles
    /// </summary>
    public bool SuppressSubtitles { get; set; }

    /// <summary>
    ///     Gets or sets the warhead state information
    /// </summary>
    public AlphaWarheadSyncInfo State { get; set; }

    /// <summary>
    ///     Gets or sets whether the event is allowed to proceed
    /// </summary>
    public bool Allowed { get; set; }

    /// <inheritdoc />
    public uint EventId { get; } = EventID;
}

/// <summary>
///     Event triggered when Alpha warhead detonation is stopped
/// </summary>
[PublicAPI]
public sealed class AlphaStopEvent : IBaseEvent
{
    private const uint EventID = AlphaEvents.Stop;

    internal AlphaStopEvent(Player? player, AlphaWarheadSyncInfo state)
    {
        Player = player ?? Server.Host;
        State = state;
        Allowed = true;
    }

    /// <summary>
    ///     Gets the player who stopped the warhead sequence
    /// </summary>
    public Player Player { get; }

    /// <summary>
    ///     Gets or sets the warhead state information
    /// </summary>
    public AlphaWarheadSyncInfo State { get; set; }

    /// <summary>
    ///     Gets or sets whether the event is allowed to proceed
    /// </summary>
    public bool Allowed { get; set; }

    /// <inheritdoc />
    public uint EventId { get; } = EventID;
}

/// <summary>
///     Event triggered when Alpha warhead detonates
/// </summary>
[PublicAPI]
public sealed class AlphaDetonateEvent : IBaseEvent
{
    private const uint EventID = AlphaEvents.Detonate;

    internal AlphaDetonateEvent()
    {
    }

    /// <inheritdoc />
    public uint EventId { get; } = EventID;
}