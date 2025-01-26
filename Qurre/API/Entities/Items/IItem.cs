using InventorySystem.Items;
using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Entities.Characters;
using UnityEngine;

namespace Qurre.API.Entities.Items;

[PublicAPI]
public interface IItem : IEntity
{
    UnityObjectWrapper<ItemBase> Base { get; }

    ItemCategory Category { get; }
    float Weight { get; }
    ItemType ItemType { get; }

    Player Owner { get; }
    IPickup? Pickup { get; }

    ushort Serial { get; }
    void Give(Player player);
    IPickup Spawn(Vector3 position, Quaternion rotation = default, Vector3 scale = default);
}