using System;
using System.Collections.Generic;
using InventorySystem.Items.Firearms.Ammo;
using InventorySystem.Items.Jailbird;
using InventorySystem.Items.ThrowableProjectiles;
using InventorySystem.Items.Usables.Scp330;
using JetBrains.Annotations;
using Qurre.API.Addons.Items;
using Qurre.API.Controllers;
using Qurre.API.Objects;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

using static ThrowableItem;

[PublicAPI]
public class PrePickupItemEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.PrePickupItem;

    internal PrePickupItemEvent(Player player, Pickup pickup)
    {
        Player = player;
        Pickup = pickup;
        Allowed = true;
    }

    public Player Player { get; }
    public Pickup Pickup { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class PickupItemEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.PickupItem;

    internal PickupItemEvent(Player player, Pickup pickup)
    {
        Player = player;
        Pickup = pickup;
        Allowed = true;
    }

    public Player Player { get; }
    public Pickup Pickup { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class PickupAmmoEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.PickupAmmo;

    internal PickupAmmoEvent(Player player, Pickup pickup, AmmoPickup ammo)
    {
        Player = player;
        Pickup = pickup;
        Ammo = ammo;
        Allowed = true;
    }

    public Player Player { get; }
    public Pickup Pickup { get; }
    public AmmoPickup Ammo { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class PickupArmorEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.PickupArmor;

    internal PickupArmorEvent(Player player, Pickup pickup)
    {
        Player = player;
        Pickup = pickup;
        Allowed = true;
    }

    public Player Player { get; }
    public Pickup Pickup { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class PickupCandyEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.PickupCandy;

    internal PickupCandyEvent(Player player, Scp330Bag bag, List<CandyKindID> list)
    {
        Player = player;
        Bag = bag;
        List = list;
        Allowed = true;
    }

    public Player Player { get; }
    public Scp330Bag Bag { get; }
    public List<CandyKindID> List { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class ThrowProjectileEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.ThrowProjectile;

    internal ThrowProjectileEvent(Player player, Item item, ProjectileSettings settings, bool fullForce)
    {
        Player = player;
        Item = item as Throwable ?? throw new ArgumentException(nameof(item));
        Settings = settings;
        FullForce = fullForce;
        Allowed = true;
    }

    public Player Player { get; }
    public Throwable Item { get; }
    public ProjectileSettings Settings { get; }
    public bool FullForce { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class DropItemEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.DropItem;

    internal DropItemEvent(Player player, Item item)
    {
        Player = player;
        Item = item;
        Allowed = true;
    }

    public Player Player { get; }
    public Item Item { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class DroppedItemEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.DroppedItem;

    internal DroppedItemEvent(Player player, Pickup pickup)
    {
        Player = player;
        Pickup = pickup;
    }

    public Player Player { get; }
    public Pickup Pickup { get; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class DropAmmoEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.DropAmmo;

    internal DropAmmoEvent(Player player, AmmoType type, ushort amount)
    {
        Player = player;
        Type = type;
        Amount = amount;
        Allowed = true;
    }

    public Player Player { get; }
    public AmmoType Type { get; set; }
    public ushort Amount { get; set; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class JailbirdTriggerEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.JailbirdTrigger;
    private Item? _item;

    internal JailbirdTriggerEvent(Player player, JailbirdItem @base, JailbirdMessageType message)
    {
        Player = player;
        JailbirdBase = @base;
        Message = message;
        Allowed = true;
    }

    public Player Player { get; }

    public Item Item
    {
        get
        {
            _item ??= Item.Get(JailbirdBase)!;
            return _item;
        }
    }

    public JailbirdItem JailbirdBase { get; }
    public JailbirdMessageType Message { get; set; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}