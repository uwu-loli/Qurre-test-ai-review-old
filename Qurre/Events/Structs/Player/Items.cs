using InventorySystem.Items.Radio;
using JetBrains.Annotations;
using Qurre.API.Entities;
using Qurre.API.Entities.Characters;
using Qurre.API.Entities.Items;
using Qurre.API.Enums;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Qurre.Events.Structs;

[PublicAPI]
public class CancelUseItemEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.CancelUseItem;

    internal CancelUseItemEvent(Player player, IItem item)
    {
        Player = player;
        Item = item;
        Allowed = true;
    }

    public Player Player { get; }
    public IItem Item { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class UseItemEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.UseItem;

    internal UseItemEvent(Player player, IItem item)
    {
        Player = player;
        Item = item;
        Allowed = true;
    }

    public Player Player { get; }
    public IItem Item { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class UsedItemEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.UsedItem;

    internal UsedItemEvent(Player player, IItem item)
    {
        Player = player;
        Item = item;
    }

    public Player Player { get; }
    public IItem Item { get; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class ChangeItemEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.ChangeItem;

    internal ChangeItemEvent(Player player, IItem? oldItem, IItem? newItem)
    {
        Player = player;
        OldItem = oldItem;
        NewItem = newItem;
        Allowed = true;
    }

    public Player Player { get; }
    public IItem? OldItem { get; }
    public IItem? NewItem { get; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class UpdateRadioEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.UpdateRadio;

    internal UpdateRadioEvent(Player player, RadioItem radioBase, RadioStatus range, bool enabled)
    {
        Player = player;
        Radio = EntityManager.GetOrException<IRadio>(radioBase);
        Range = range;
        Enabled = enabled;
        Allowed = true;
    }

    public Player Player { get; }
    public IRadio Radio { get; }
    public RadioStatus Range { get; set; }
    public bool Enabled { get; set; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}

[PublicAPI]
public class UsingRadioEvent : IBaseEvent
{
    private const uint EventID = PlayerEvents.UsingRadio;

    internal UsingRadioEvent(Player player, RadioItem radioBase, float num)
    {
        Player = player;
        Radio = EntityManager.GetOrException<IRadio>(radioBase);
        Battery = radioBase._battery * 100;
        Consumption = Time.deltaTime * (num / 60 / 100) * 100;
        Allowed = true;
    }

    public Player Player { get; }
    public IRadio Radio { get; }
    public float Battery { get; set; }
    public float Consumption { get; set; }
    public bool Allowed { get; set; }
    public uint EventId { get; } = EventID;
}