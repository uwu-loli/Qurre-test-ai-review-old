using InventorySystem;
using InventorySystem.Items.Pickups;
using JetBrains.Annotations;
using PlayerStatsSystem;
using Qurre.API.Entities.Characters;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class CreatePickupEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.CreatePickup;

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
    
    public PickupSyncInfo Info { get; }
    public Inventory Inventory { get; }
    
    internal CreatePickupEvent(PickupSyncInfo psi, Inventory inv)
    {
        Info = psi;
        Inventory = inv;
    }
}

[PublicAPI]
public class CorpseSpawnEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.CorpseSpawn;

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
    
    public Player Owner { get; }
    public DamageHandlerBase Handler { get; }
    
    internal CorpseSpawnEvent(Player owner, DamageHandlerBase handler)
    {
        Owner = owner;
        Handler = handler;
    }
}

[PublicAPI]
public class CorpseSpawnedEvent : IBaseEvent
{
    private const uint EventID = MapEvents.CorpseSpawned;

    public ICorpse Corpse { get; }
    public uint EventId { get; } = EventID;
    
    internal CorpseSpawnedEvent(ICorpse corpse)
    {
        Corpse = corpse;
    }
}