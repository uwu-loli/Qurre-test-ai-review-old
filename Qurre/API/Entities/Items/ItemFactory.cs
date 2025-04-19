using System.Collections.Generic;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using JetBrains.Annotations;
using UnityEngine;

namespace Qurre.API.Entities.Items;

[PublicAPI]
public class ItemFactory
{
    #region Pickup Factory

    public IPickup CreatePickup(ItemType itemType,
        Vector3 position,
        Quaternion? rotation = null,
        Vector3? scale = null,
        bool isKinematic = false,
        bool useGravity = true,
        ushort? serial = null,
        bool inUse = false,
        bool isLocked = false,
        bool doSpawn = true)
    {
        if (!InventoryItemLoader.AvailableItems.TryGetValue(itemType, out var itemBase))
            throw new KeyNotFoundException($"InventoryItemLoader.AvailableItems[{itemType}] not found.");

        var spawnRotation = rotation ?? Quaternion.identity;
        var spawnScale = scale ?? Vector3.one;

        var pickupInstance = Object.Instantiate(itemBase.PickupDropModel, position, spawnRotation);
        pickupInstance.transform.localScale = spawnScale;

        pickupInstance.NetworkInfo = new PickupSyncInfo
        {
            ItemId = itemType,
            Serial = serial ?? ItemSerialGenerator.GenerateNext(),
            WeightKg = itemBase.Weight,
            InUse = inUse,
            Locked = isLocked
        };

        if (pickupInstance.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = isKinematic;
            rigidbody.useGravity = useGravity;
        }

        var pickup = EntityManager.GetOrException<IPickup>(pickupInstance);
        if (doSpawn) pickup.Spawn();
        else pickup.UnSpawn();
        return pickup;
    }

    public IPickup CreatePickup(ItemType itemType,
        Vector3 position,
        Vector3 eulerAngles,
        Vector3? scale = null,
        bool isKinematic = false,
        bool useGravity = true,
        ushort? serial = null,
        bool inUse = false,
        bool isLocked = false,
        bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreatePickup(itemType, position, rotationQuaternion, scale, isKinematic, useGravity, serial, inUse,
            isLocked, doSpawn);
    }

    #endregion
}