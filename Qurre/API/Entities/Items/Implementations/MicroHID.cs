using InventorySystem.Items.MicroHID;
using Qurre.API.Core;
using Qurre.Internal.Attributes;

namespace Qurre.API.Entities.Items.Implementations;

[EntityWrapBindForFactory(typeof(MicroHIDItem))]
internal sealed class MicroHID(MicroHIDItem itemBase) : Item(itemBase), IMicroHID
{
    /// <inheritdoc />
    public new UnityObjectWrapper<MicroHIDItem> Base { get; } = itemBase;

    /// <inheritdoc />
    public float Energy
    {
        get => Base.Instance.EnergyManager.Energy;
        set => Base.Instance.EnergyManager.ServerSetEnergy(Serial, value);
    }
}