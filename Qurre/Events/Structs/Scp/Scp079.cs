using JetBrains.Annotations;
using PlayerRoles.PlayableScps.Scp079;
using Qurre.API.Controllers;
using Qurre.API.Entities.Characters;
using Qurre.API.Entities.Structures;
using Qurre.API.World.Entities.Player;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class ActivateGeneratorEvent : ICancellableEvent
{
    private const uint EventID = ScpEvents.ActivateGenerator;

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
    
    public IGenerator Generator { get; }
    
    internal ActivateGeneratorEvent(IGenerator generator)
    {
        Generator = generator;
    }
}

[PublicAPI]
public class Scp079GetExpEvent : ICancellableEvent
{
    private const uint EventID = ScpEvents.Scp079GetExp;

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
    
    public Player Player { get; }
    public Scp079HudTranslation Type { get; }
    public int Amount { get; set; }
    
    internal Scp079GetExpEvent(Player player, Scp079HudTranslation type, int amount)
    {
        Player = player;
        Type = type;
        Amount = amount;
    }
}

[PublicAPI]
public class Scp079NewLvlEvent : ICancellableEvent
{
    private const uint EventID = ScpEvents.Scp079NewLvl;

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
    
    public Player Player { get; }
    public int Level { get; set; }
    
    internal Scp079NewLvlEvent(Player player, int level)
    {
        Player = player;
        Level = level;
    }
}

[PublicAPI]
public class Scp079RecontainEvent : IBaseEvent
{
    private const uint EventID = ScpEvents.Scp079Recontain;

    public uint EventId { get; } = EventID;
    
    internal Scp079RecontainEvent()
    {
    }
}

[PublicAPI]
public class GeneratorStatusEvent : IBaseEvent
{
    private const uint EventID = ScpEvents.GeneratorStatus;

    public uint EventId { get; } = EventID;
    
    public int EnragedCount { get; }
    public int TotalCount { get; }
    
    internal GeneratorStatusEvent(int enragedCount, int totalCount)
    {
        EnragedCount = enragedCount;
        TotalCount = totalCount;
    }
}
