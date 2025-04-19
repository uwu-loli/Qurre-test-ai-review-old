using InventorySystem.Items.Firearms.Ammo;
using Qurre.API.Core;
using Qurre.Internal.Attributes;

namespace Qurre.API.Entities.Items.Implementations;

[EntityWrapBindForFactory(typeof(AmmoItem))]
internal sealed class Ammo(AmmoItem itemBase) : Item(itemBase), IAmmo
{
    /// <inheritdoc />
    public new UnityObjectWrapper<AmmoItem> Base { get; } = itemBase;

    /// <inheritdoc />
    public int UnitPrice
    {
        get => Base.Instance.UnitPrice;
        set => Base.Instance.UnitPrice = value;
    }
}