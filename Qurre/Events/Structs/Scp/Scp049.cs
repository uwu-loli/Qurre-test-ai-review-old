using JetBrains.Annotations;
using PlayerRoles.Ragdolls;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Characters;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class Scp049RaisingStartEvent : ICancellableEvent
{
    private const uint EventID = ScpEvents.Scp049RaisingStart;

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
    
    public Player Issuer { get; }
    public Player Target { get; }
    public ICorpse Corpse { get; }
    
    internal Scp049RaisingStartEvent(Player issuer, Player target, BasicRagdoll basicRagdoll)
    {
        Issuer = issuer;
        Target = target;
        Corpse = basicRagdoll.GetCorpse()!;
    }
}

[PublicAPI]
public class Scp049RaisingEndEvent : ICancellableEvent
{
    private const uint EventID = ScpEvents.Scp049RaisingEnd;

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
    
    public Player Issuer { get; }
    public Player Target { get; }
    public ICorpse Corpse { get; }
    
    internal Scp049RaisingEndEvent(Player issuer, Player target, BasicRagdoll basicRagdoll)
    {
        Issuer = issuer;
        Target = target;
        Corpse = EntityManager.GetOrException<ICorpse>(basicRagdoll);
    }
}