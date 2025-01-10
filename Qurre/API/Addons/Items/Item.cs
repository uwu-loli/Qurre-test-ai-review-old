using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using InventorySystem.Items;
using InventorySystem.Items.Armor;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Ammo;
using InventorySystem.Items.Keycards;
using InventorySystem.Items.MicroHID;
using InventorySystem.Items.Radio;
using InventorySystem.Items.ThrowableProjectiles;
using InventorySystem.Items.ToggleableLights.Flashlight;
using InventorySystem.Items.Usables;
using JetBrains.Annotations;
using Mirror;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Addons.Items;

[PublicAPI]
public class Item : Entity<ItemBase, Item>
{
    protected override ItemBase UnsafeBase { get; }
    
    public ItemCategory Category => Base.Category;
    public float Weight => Base.Weight;
    public ItemType ItemType => Base.ItemTypeId;

    public Player Owner => (bool)Base.Owner ? Base.Owner.GetPlayer() ?? Server.Host : Server.Host;
    public Pickup? Pickup => Pickup.Get(Base.PickupDropModel);

    public ushort Serial
    {
        get => Base.ItemSerial;
        internal set
        {
            if (!Base.PickupDropModel) return;
            var syncInfo = Base.PickupDropModel.NetworkInfo;
            syncInfo.Serial = value != 0 ? value : ItemSerialGenerator.GenerateNext();
            Base.PickupDropModel.NetworkInfo = syncInfo;
        }
    }

    protected Item(ItemBase itemBase)
    {
        UnsafeBase = itemBase;
        
        if (Base.OwnerInventory)
            Serial = Base.OwnerInventory.UserInventory.Items.FirstOrDefault(i => i.Value == Base).Key;
        
        if (Serial == 0)
            Serial = ItemSerialGenerator.GenerateNext();

        BaseToWrap[itemBase] = this;
        AddEntityLink();
    }

    public Item(ItemType type)
        : this(Server.InventoryHost.CreateItemInstance(new ItemIdentifier(type, ItemSerialGenerator.GenerateNext()),
            true))
    {
    }

    public void Give(Player player)
    {
        player.Inventory.AddItem(Base);
    }

    public virtual Pickup Spawn(Vector3 position, Quaternion rotation = default, Vector3 scale = default)
    {
        var ipb = Object.Instantiate(Base.PickupDropModel, position, rotation);

        ipb.Info.ItemId = ItemType;
        ipb.Info.WeightKg = Weight;
        ipb.NetworkInfo = ipb.Info;

        ipb.Position = position;
        ipb.Rotation = rotation;

        NetworkServer.Spawn(ipb.gameObject);

        var pickup = Pickup.Get(ipb);

        if (pickup is null)
            throw new NullReferenceException("Pickup could not be found");

        pickup.WorldScale = scale == default ? Vector3.one : scale;
        return pickup;
    }

    public static Item? Get(ItemBase itemBase)
    {
        if (!itemBase) return null;
        if (BaseToWrap.TryGetValue(itemBase, out var item))
            return item;

        return itemBase switch
        {
            Firearm gun => new Gun(gun),
            KeycardItem card => new Keycard(card),
            UsableItem usable =>
                //if (usable is Scp330Bag bag)
                //    return new Scp330(bag);
                new Usable(usable),
            RadioItem radio => new Radio(radio),
            MicroHIDItem hid => new MicroHID(hid),
            BodyArmor armor => new Armor(armor),
            AmmoItem ammo => new Ammo(ammo),
            FlashlightItem flashlight => new Flashlight(flashlight),
            ThrowableItem throwable => throwable.Projectile switch
            {
                FlashbangGrenade _ => new GrenadeFlash(throwable),
                ExplosionGrenade _ => new GrenadeFrag(throwable),
                _ => new Throwable(throwable)
            },
            _ => new Item(itemBase)
        };
    }

    public static Item? Get(ushort serial)
    {
        return Object.FindObjectsOfType<ItemBase>().TryFind(out var item, x => x.ItemSerial == serial)
            ? Get(item)
            : null;
    }

    public static bool TryGet(ItemBase itemBase, [NotNullWhen(true)] out Item? item)
    {
        item = Get(itemBase);
        return item is not null;
    }

    public static bool TryGet(ushort serial, [NotNullWhen(true)] out Item? item)
    {
        item = Get(serial);
        return item is not null;
    }

    public override bool Equals(object? obj)
    {
        return obj is Item item && Serial == item.Serial;
    }

    public override int GetHashCode()
    {
        return -29014143 + Serial.GetHashCode();
    }

    public static bool operator ==(Item? a, Item? b)
    {
        return a?.Serial == b?.Serial;
    }

    public static bool operator !=(Item a, Item b)
    {
        return !(a == b);
    }
}