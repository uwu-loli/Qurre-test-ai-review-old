using InventorySystem.Items.Usables;
using Qurre.API.Core;
using Qurre.Internal.Attributes;

namespace Qurre.API.Entities.Items.Implementations;

[EntityWrapBindForFactory(typeof(UsableItem))]
internal sealed class Usable(UsableItem itemBase) : Item(itemBase), IUsable
{
    public new UnityObjectWrapper<UsableItem> Base { get; } = itemBase;

    public bool IsEquippable => Base.Instance.AllowEquip;
    public bool IsHolsterable => Base.Instance.AllowHolster;

    public float UseTime
    {
        get => Base.Instance.UseTime;
        set => Base.Instance.UseTime = value;
    }

    public float MaxCancellableTime
    {
        get => Base.Instance.MaxCancellableTime;
        set => Base.Instance.MaxCancellableTime = value;
    }

    public float RemainingCooldown
    {
        get => Base.Instance.RemainingCooldown;
        set => Base.Instance.RemainingCooldown = value;
    }
}