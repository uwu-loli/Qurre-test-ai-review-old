using System;
using System.Diagnostics.CodeAnalysis;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using JetBrains.Annotations;
using Mirror;
using Qurre.API.Controllers.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Addons.Items;

/// <summary>
/// Represents a pickup-able item in the game.
/// Provides an abstraction over the base game's item pickups.
/// </summary>
[PublicAPI]
public class Pickup : NetworkEntity<ItemPickupBase, Pickup>
{
    protected sealed override ItemPickupBase UnsafeBase { get; }

    /// <summary>
    /// Gets the <see cref="ItemCategory"/> of this pickup.
    /// </summary>
    public ItemCategory ItemCategory { get; private set; }

    /// <summary>
    /// Gets or sets the <see cref="ItemType"/> of this pickup.
    /// </summary>
    public ItemType ItemType
    {
        get => Base.NetworkInfo.ItemId;
        set
        {
            var syncInfo = Base.NetworkInfo;
            syncInfo.ItemId = value;
            Base.NetworkInfo = syncInfo;
            ItemCategory = value.GetCategory();
        }
    }

    /// <summary>
    /// Gets or sets the serial number of this pickup.
    /// This is a unique identifier for the pickup instance.
    /// </summary>
    public ushort Serial
    {
        get => Base.NetworkInfo.Serial;
        set
        {
            if (value == 0) value = ItemSerialGenerator.GenerateNext();
            var syncInfo = Base.NetworkInfo;
            syncInfo.Serial = value;
            Base.NetworkInfo = syncInfo;
        }
    }

    /// <summary>
    /// Gets or sets the weight of this pickup in kilograms.
    /// </summary>
    public float Weight
    {
        get => Base.NetworkInfo.WeightKg;
        set
        {
            var syncInfo = Base.NetworkInfo;
            syncInfo.WeightKg = value;
            Base.NetworkInfo = syncInfo;
        }
    }

    /// <summary>
    /// Gets or sets whether the pickup is locked.
    /// A locked pickup cannot be interacted with.
    /// </summary>
    public bool IsLocked
    {
        get => Base.NetworkInfo.Locked;
        set
        {
            var info = Base.NetworkInfo;
            info.Locked = value;
            Base.NetworkInfo = info;
        }
    }

    /// <summary>
    /// Gets or sets whether the pickup is currently in use.
    /// </summary>
    public bool InUse
    {
        get => Base.NetworkInfo.InUse;
        set
        {
            var info = Base.NetworkInfo;
            info.InUse = value;
            Base.NetworkInfo = info;
        }
    }
    
    /// <summary>
    /// Initializes a new <see cref="Pickup"/> instance from an existing <see cref="ItemPickupBase"/>.
    /// </summary>
    /// <param name="pickupBase">The base game pickup to wrap.</param>
    private Pickup(ItemPickupBase pickupBase)
    {
        UnsafeBase = pickupBase;
        ItemCategory = ItemType.GetCategory();
        
        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    /// <summary>
    /// Initializes a new <see cref="Pickup"/> instance with specified properties.
    /// </summary>
    /// <param name="itemType">The <see cref="ItemType"/> of the pickup.</param>
    /// <param name="position">The position of the pickup. Default is <see cref="Vector3.zero"/>.</param>
    /// <param name="rotation">The rotation of the pickup. Default is <see cref="Quaternion.identity"/>.</param>
    /// <param name="scale">The scale of the pickup. Default is <see cref="Vector3.one"/>.</param>
    /// <param name="isLocked">Whether the pickup is locked. Default is <see langword="false"/>.</param>
    /// <param name="inUse">Whether the pickup is in use. Default is <see langword="false"/>.</param>
    /// <param name="weightKg">The weight of the pickup in kilograms. Default is the item's default weight.</param>
    /// <param name="doSpawn">Whether to spawn the pickup on the network. Default is <see langword="true"/>.</param>
    public Pickup(ItemType itemType,
        Vector3? position = null,
        Quaternion? rotation = null,
        Vector3? scale = null,
        bool isLocked = false,
        bool inUse = false,
        float? weightKg = null,
        bool doSpawn = true)
    {
        if (!InventoryItemLoader.AvailableItems.TryGetValue(itemType, out var itemBase))
            throw new ArgumentException($"Unknown item type: {itemType}");

        var instancePickupBase = Object.Instantiate(itemBase.PickupDropModel, position ?? Vector3.zero, rotation ?? Quaternion.identity);
        instancePickupBase.transform.localScale = scale ?? Vector3.one;
        instancePickupBase.NetworkInfo = new PickupSyncInfo
        {
            ItemId = itemType,
            WeightKg = weightKg ?? itemBase.Weight,
            Serial = ItemSerialGenerator.GenerateNext(),
            Locked = isLocked,
            InUse = inUse
        };
        
        UnsafeBase = instancePickupBase;
        ItemCategory = ItemType.GetCategory();
        
        BaseToWrap[Base] = this;
        AddEntityLink();
        
        if (doSpawn) NetworkServer.Spawn(instancePickupBase.gameObject);
    }
    
    /// <summary>
    /// Retrieves a <see cref="Pickup"/> instance from an <see cref="ItemPickupBase"/>.
    /// </summary>
    /// <param name="pickupBase">The base game pickup to retrieve the instance for.</param>
    /// <returns>The corresponding <see cref="Pickup"/> instance, or <see langword="null"/> if not found.</returns>
    public static Pickup? Get(ItemPickupBase pickupBase)
    {
        if (!pickupBase) return null;
        return BaseToWrap.TryGetValue(pickupBase, out var pickup) ? pickup : new Pickup(pickupBase);
    }

    public static bool TryGet(ItemPickupBase pickupBase, [NotNullWhen(true)] out Pickup? pickup)
    {
        pickup = Get(pickupBase);
        return pickup != null;
    }
}
