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

    internal CreatePickupEvent(PickupSyncInfo psi, Inventory inv)
    {
        Info = psi;
        Inventory = inv;
    }

    public PickupSyncInfo Info { get; }
    public Inventory Inventory { get; }

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
}

[PublicAPI]
public class CorpseSpawnEvent : ICancellableEvent
{
    private const uint EventID = MapEvents.CorpseSpawn;

    internal CorpseSpawnEvent(Player owner, DamageHandlerBase handler)
    {
        Owner = owner;
        Handler = handler;
    }

    public Player Owner { get; }
    public DamageHandlerBase Handler { get; }

    public uint EventId { get; } = EventID;
    public bool IsAllowed { get; set; } = true;
}

[PublicAPI]
public class CorpseSpawnedEvent : IBaseEvent
{
    private const uint EventID = MapEvents.CorpseSpawned;

    internal CorpseSpawnedEvent(ICorpse corpse)
    {
        Corpse = corpse;
    }

    public ICorpse Corpse { get; }
    public uint EventId { get; } = EventID;
}