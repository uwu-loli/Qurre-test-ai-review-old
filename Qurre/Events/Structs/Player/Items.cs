using System;
using InventorySystem.Items.Radio;
using JetBrains.Annotations;
using Qurre.API.Addons.Items;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class CancelUseItemEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.CancelUseItem;

    internal CancelUseItemEvent(Player player, Item item)
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
public class UseItemEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.UseItem;

    internal UseItemEvent(Player player, Item item)
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
public class UsedItemEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.UsedItem;

    internal UsedItemEvent(Player player, Item item)
    {
        Player = player;
        Item = item;
    }

    public Player Player { get; }
    public Item Item { get; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class ChangeItemEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.ChangeItem;

    internal ChangeItemEvent(Player player, Item? oldItem, Item? newItem)
    {
        Player = player;
        OldItem = oldItem;
        NewItem = newItem;
        Allowed = true;
    }

    public Player Player { get; }
    public Item? OldItem { get; }
    public Item? NewItem { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class UpdateRadioEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.UpdateRadio;

    internal UpdateRadioEvent(Player player, RadioItem radio, RadioStatus range, bool enabled)
    {
        Player = player;
        Radio = Item.Get(radio) as Radio ?? throw new ArgumentNullException(nameof(radio));
        Range = range;
        Enabled = enabled;
        Allowed = true;
    }

    public Player Player { get; }
    public Radio Radio { get; }
    public RadioStatus Range { get; set; }
    public bool Enabled { get; set; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class UsingRadioEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.UsingRadio;

    internal UsingRadioEvent(Player player, RadioItem radio, float num)
    {
        Player = player;
        Radio = Item.Get(radio) as Radio ?? throw new ArgumentNullException(nameof(radio));
        Battery = radio._battery * 100;
        Consumption = Time.deltaTime * (num / 60 / 100) * 100;
        Allowed = true;
    }

    public Player Player { get; }
    public Radio Radio { get; }
    public float Battery { get; set; }
    public float Consumption { get; set; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}