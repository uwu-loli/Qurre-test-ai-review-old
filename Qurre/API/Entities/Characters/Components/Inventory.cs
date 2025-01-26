using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Pickups;
using JetBrains.Annotations;
using Qurre.API.Entities.Characters.Components.Structs;
using Qurre.API.Entities.Items;
using Qurre.API.Enums;

namespace Qurre.API.Entities.Characters.Components;

[PublicAPI]
public sealed class Inventory
{
    private readonly Player _player;

    internal Inventory(Player player)
    {
        Base = player.ReferenceHub.inventory;
        Ammo = new AmmoBox(player);
        Hand = new Hand(player);
        _player = player;
    }

    public InventorySystem.Inventory Base { get; }
    public AmmoBox Ammo { get; }
    public Hand Hand { get; }

    public int ItemsCount => Base.UserInventory.Items.Count;

    public Dictionary<ushort, IItem> Items
    {
        get
        {
            Dictionary<ushort, IItem> dict = [];
            foreach (var preItem in Base.UserInventory.Items)
            {
                var item = EntityManager.Get<IItem>(preItem.Value);
                if (item is not null)
                    dict.Add(preItem.Key, item);
            }

            return dict;
        }
        set
        {
            Dictionary<ushort, ItemBase> dict = [];

            foreach (var preItem in value)
                dict.Add(preItem.Key, preItem.Value.Base.Instance);

            Base.UserInventory.Items = dict;
            Base.SendItemsNextFrame = true;
        }
    }

    public bool HasItem(ItemType item)
    {
        return Base.UserInventory.Items.Any(tempItem => tempItem.Value.ItemTypeId == item);
    }

    public void Reset(IEnumerable<IItem> newItems)
    {
        Clear();

        foreach (var item in newItems)
            AddItem(item);
    }

    public void Reset(IEnumerable<ItemBase> newItems)
    {
        Clear();

        foreach (var item in newItems)
            AddItem(item);
    }

    public void Reset(IEnumerable<ItemType> newItems)
    {
        Clear();

        foreach (var type in newItems)
            AddItem(type);
    }

    public void Clear()
    {
        Clear(true);
    }

    public void Clear(bool clearAmmo)
    {
        if (clearAmmo)
        {
            Ammo[AmmoTypes.Ammo556] = 0;
            Ammo[AmmoTypes.Ammo762] = 0;
            Ammo[AmmoTypes.Ammo9] = 0;
            Ammo[AmmoTypes.Ammo12Gauge] = 0;
            Ammo[AmmoTypes.Ammo44Cal] = 0;
        }

        while (Base.UserInventory.Items.Count != 0)
            Base.ServerRemoveItem(Base.UserInventory.Items.ElementAt(0).Key, null);
    }

    public void DropAllAmmo()
    {
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var item in Base.UserInventory.ReserveAmmo)
            if (item.Value != 0)
                Base.ServerDropAmmo(item.Key, ushort.MaxValue);
    }

    public void DropAllItems()
    {
        while (Base.UserInventory.Items.Count != 0)
            Base.ServerDropItem(Base.UserInventory.Items.First().Key);
    }

    public void DropAll()
    {
        Base.ServerDropEverything();
    }

    public void SelectItem(ushort serial)
    {
        Base.ServerSelectItem(serial);
    }

    public void SelectItem(IItem item)
    {
        SelectItem(item.Serial);
    }

    public void DropItem(ushort serial)
    {
        Base.ServerDropItem(serial);
    }

    public void DropItem(IItem item)
    {
        Base.ServerDropItem(item.Serial);
    }

    public IItem? AddItem(ItemBase itemBase)
    {
        if (itemBase == null)
            return null;

        if (itemBase.PickupDropModel == null)
            return null;

        Base.UserInventory.Items[itemBase.PickupDropModel.NetworkInfo.Serial] = itemBase;
        itemBase.OnAdded(itemBase.PickupDropModel);

        if (itemBase is Firearm firearm)
            SetupFirearmAttachments(_player.ReferenceHub, firearm);

        Base.SendItemsNextFrame = true;

        return EntityManager.Get<IItem>(itemBase);
    }

    public void AddItem(IItem item)
    {
        AddItem(item.Base.Instance);
    }

    public void AddItem(IItem item, uint amount)
    {
        if (amount == 0)
            return;

        for (uint i = 0; i < amount; i++)
            AddItem(item);
    }

    public IItem? AddItem(ItemType itemType)
    {
        var itemBase = Base.ServerAddItem(itemType, ItemAddReason.Undefined);
        if (itemBase is Firearm firearm)
            SetupFirearmAttachments(_player.ReferenceHub, firearm);
        return EntityManager.Get<IItem>(itemBase);
    }

    public void AddItem(ItemType itemType, uint amount)
    {
        if (amount == 0)
            return;

        for (uint i = 0; i < amount; i++)
            AddItem(itemType);
    }

    public void AddItem(IEnumerable<IItem> items)
    {
        foreach (var item in items)
            AddItem(item);
    }

    public void RemoveItem(ushort serial, ItemPickupBase itemPickupBase)
    {
        Base.ServerRemoveItem(serial, itemPickupBase);
    }

    public void RemoveItem(IPickup pickup)
    {
        Base.ServerRemoveItem(pickup.Serial, pickup.Base.Instance);
    }

    public void RemoveItem(IItem item)
    {
        if (item.Pickup == null)
            return;

        Base.ServerRemoveItem(item.Serial, item.Pickup.Base.Instance);
    }


    private static void SetupFirearmAttachments(ReferenceHub referenceHub, Firearm firearm)
    {
        if (!AttachmentsServerHandler.PlayerPreferences.TryGetValue(referenceHub, out var itemPreferences) ||
            !itemPreferences.TryGetValue(firearm.ItemTypeId, out var attachmentsCode))
            attachmentsCode = 0U;

        firearm.ApplyAttachmentsCode(attachmentsCode, true);
        firearm.ServerResendAttachmentCode();
        AttachmentsServerHandler.ServerApplyPreference(referenceHub, firearm.ItemTypeId, attachmentsCode);
    }
}