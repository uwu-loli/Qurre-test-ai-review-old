using JetBrains.Annotations;
using PlayerRoles.PlayableScps.Scp079;
using Qurre.API.Entities.Characters;
using Qurre.API.Entities.Structures;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class ActivateGeneratorEvent : ICancellableEvent
{
    private const uint EventID = ScpEvents.ActivateGenerator;

    internal ActivateGeneratorEvent(IGenerator generator)
    {
        Generator = generator;
    }

    public IGenerator Generator { get; }

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
}

[PublicAPI]
public class Scp079GetExpEvent : ICancellableEvent
{
    private const uint EventID = ScpEvents.Scp079GetExp;

    internal Scp079GetExpEvent(Player player, Scp079HudTranslation type, int amount)
    {
        Player = player;
        Type = type;
        Amount = amount;
    }

    public Player Player { get; }
    public Scp079HudTranslation Type { get; }
    public int Amount { get; set; }

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
}

[PublicAPI]
public class Scp079NewLvlEvent : ICancellableEvent
{
    private const uint EventID = ScpEvents.Scp079NewLvl;

    internal Scp079NewLvlEvent(Player player, int level)
    {
        Player = player;
        Level = level;
    }

    public Player Player { get; }
    public int Level { get; set; }

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
}

[PublicAPI]
public class Scp079RecontainEvent : IBaseEvent
{
    private const uint EventID = ScpEvents.Scp079Recontain;

    internal Scp079RecontainEvent()
    {
    }

    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class GeneratorStatusEvent : IBaseEvent
{
    private const uint EventID = ScpEvents.GeneratorStatus;

    internal GeneratorStatusEvent(int enragedCount, int totalCount)
    {
        EnragedCount = enragedCount;
        TotalCount = totalCount;
    }

    public int EnragedCount { get; }
    public int TotalCount { get; }

    public uint EventId { get; } = EventID;
}