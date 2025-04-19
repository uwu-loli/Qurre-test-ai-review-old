using System;
using System.Linq;
using InventorySystem.Items;
using Mirror;
using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.API.Entities.Characters;
using Qurre.Internal.Attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Entities.Items.Implementations;

[EntityWrapBindForFactory(typeof(ItemBase))]
internal class Item : Entity, IItem
{
    public Item(ItemBase itemBase) : base(itemBase.gameObject)
    {
        Base = itemBase;

        if (Base.Instance.OwnerInventory)
            Serial = Base.Instance.OwnerInventory.UserInventory.Items.FirstOrDefault(i => i.Value == Base.Instance).Key;

        if (Serial == 0)
            Serial = ItemSerialGenerator.GenerateNext();
    }

    public Item(ItemType type)
        : this(Server.InventoryHost.CreateItemInstance(new ItemIdentifier(type, ItemSerialGenerator.GenerateNext()),
            true))
    {
    }

    public UnityObjectWrapper<ItemBase> Base { get; }

    public ItemCategory Category => Base.Instance.Category;
    public float Weight => Base.Instance.Weight;
    public ItemType ItemType => Base.Instance.ItemTypeId;

    public Player Owner => (bool)Base.Instance.Owner ? Base.Instance.Owner.GetPlayer() ?? Server.Host : Server.Host;
    public IPickup? Pickup => EntityManager.Get<IPickup>(Base.Instance.PickupDropModel);

    public ushort Serial
    {
        get => Base.Instance.ItemSerial;
        private init
        {
            if (!Base.Instance.PickupDropModel) return;
            var syncInfo = Base.Instance.PickupDropModel.NetworkInfo;
            syncInfo.Serial = value != 0 ? value : ItemSerialGenerator.GenerateNext();
            Base.Instance.PickupDropModel.NetworkInfo = syncInfo;
        }
    }

    public void Give(Player player)
    {
        player.Inventory.AddItem(Base.Instance);
    }

    public IPickup Spawn(Vector3 position, Quaternion rotation = default, Vector3 scale = default)
    {
        var ipb = Object.Instantiate(Base.Instance.PickupDropModel, position, rotation);

        ipb.Info.ItemId = ItemType;
        ipb.Info.WeightKg = Weight;
        ipb.NetworkInfo = ipb.Info;

        ipb.Position = position;
        ipb.Rotation = rotation;

        NetworkServer.Spawn(ipb.gameObject);

        var pickup = EntityManager.Get<IPickup>(ipb);

        if (pickup is null)
            throw new NullReferenceException("Pickup could not be found");

        pickup.WorldScale = scale == default ? Vector3.one : scale;
        return pickup;
    }
}