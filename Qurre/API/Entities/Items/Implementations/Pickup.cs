using System;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using Mirror;
using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.Internal.Attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Entities.Items.Implementations;

/// <summary>
///     Represents a pickup-able item in the game.
///     Provides an abstraction over the base game's item pickups.
/// </summary>
[EntityWrapBindForFactory(typeof(ItemPickupBase))]
internal class Pickup : NetworkEntity, IPickup
{
    /// <summary>
    ///     Initializes a new <see cref="Pickup" /> instance from an existing <see cref="ItemPickupBase" />.
    /// </summary>
    /// <param name="pickupBase">The base game pickup to wrap.</param>
    public Pickup(ItemPickupBase pickupBase) : base(pickupBase.gameObject)
    {
        Base = pickupBase;
        ItemCategory = ItemType.GetCategory();
    }

    public UnityObjectWrapper<ItemPickupBase> Base { get; }

    /// <summary>
    ///     Gets the <see cref="ItemCategory" /> of this pickup.
    /// </summary>
    public ItemCategory ItemCategory { get; private set; }

    /// <summary>
    ///     Gets or sets the <see cref="ItemType" /> of this pickup.
    /// </summary>
    public ItemType ItemType
    {
        get => Base.Instance.NetworkInfo.ItemId;
        set
        {
            var syncInfo = Base.Instance.NetworkInfo;
            syncInfo.ItemId = value;
            Base.Instance.NetworkInfo = syncInfo;
            ItemCategory = value.GetCategory();
        }
    }

    /// <summary>
    ///     Gets or sets the serial number of this pickup.
    ///     This is a unique identifier for the pickup instance.
    /// </summary>
    public ushort Serial
    {
        get => Base.Instance.NetworkInfo.Serial;
        set
        {
            if (value == 0) value = ItemSerialGenerator.GenerateNext();
            var syncInfo = Base.Instance.NetworkInfo;
            syncInfo.Serial = value;
            Base.Instance.NetworkInfo = syncInfo;
        }
    }

    /// <summary>
    ///     Gets or sets the weight of this pickup in kilograms.
    /// </summary>
    public float Weight
    {
        get => Base.Instance.NetworkInfo.WeightKg;
        set
        {
            var syncInfo = Base.Instance.NetworkInfo;
            syncInfo.WeightKg = value;
            Base.Instance.NetworkInfo = syncInfo;
        }
    }

    /// <summary>
    ///     Gets or sets whether the pickup is locked.
    ///     A locked pickup cannot be interacted with.
    /// </summary>
    public bool IsLocked
    {
        get => Base.Instance.NetworkInfo.Locked;
        set
        {
            var info = Base.Instance.NetworkInfo;
            info.Locked = value;
            Base.Instance.NetworkInfo = info;
        }
    }

    /// <summary>
    ///     Gets or sets whether the pickup is currently in use.
    /// </summary>
    public bool InUse
    {
        get => Base.Instance.NetworkInfo.InUse;
        set
        {
            var info = Base.Instance.NetworkInfo;
            info.InUse = value;
            Base.Instance.NetworkInfo = info;
        }
    }

    /// <summary>
    ///     Initializes a new <see cref="Pickup" /> instance with specified properties.
    /// </summary>
    /// <param name="itemType">The <see cref="ItemType" /> of the pickup.</param>
    /// <param name="position">The position of the pickup. Default is <see cref="Vector3.zero" />.</param>
    /// <param name="rotation">The rotation of the pickup. Default is <see cref="Quaternion.identity" />.</param>
    /// <param name="scale">The scale of the pickup. Default is <see cref="Vector3.one" />.</param>
    /// <param name="isLocked">Whether the pickup is locked. Default is <see langword="false" />.</param>
    /// <param name="inUse">Whether the pickup is in use. Default is <see langword="false" />.</param>
    /// <param name="weightKg">The weight of the pickup in kilograms. Default is the item's default weight.</param>
    /// <param name="doSpawn">Whether to spawn the pickup on the network. Default is <see langword="true" />.</param>
    public static Pickup Create(ItemType itemType,
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

        var instancePickupBase = Object.Instantiate(itemBase.PickupDropModel, position ?? Vector3.zero,
            rotation ?? Quaternion.identity);
        instancePickupBase.transform.localScale = scale ?? Vector3.one;
        instancePickupBase.NetworkInfo = new PickupSyncInfo
        {
            ItemId = itemType,
            WeightKg = weightKg ?? itemBase.Weight,
            Serial = ItemSerialGenerator.GenerateNext(),
            Locked = isLocked,
            InUse = inUse
        };

        var pickup = new Pickup(instancePickupBase);
        if (doSpawn) NetworkServer.Spawn(instancePickupBase.gameObject);
        return pickup;
    }
}