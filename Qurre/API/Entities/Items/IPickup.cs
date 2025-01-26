using InventorySystem.Items.Pickups;
using JetBrains.Annotations;
using Qurre.API.Core;

namespace Qurre.API.Entities.Items;

[PublicAPI]
public interface IPickup : INetworkEntity
{
    UnityObjectWrapper<ItemPickupBase> Base { get; }

    ItemCategory ItemCategory { get; }
    ItemType ItemType { get; set; }
    ushort Serial { get; set; }
    float Weight { get; set; }
    bool IsLocked { get; set; }
    bool InUse { get; set; }
}