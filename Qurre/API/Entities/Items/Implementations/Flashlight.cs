using InventorySystem.Items.ToggleableLights;
using InventorySystem.Items.ToggleableLights.Flashlight;
using Qurre.API.Core;
using Qurre.Internal.Attributes;
using Utils.Networking;

namespace Qurre.API.Entities.Items.Implementations;

[EntityWrapBindForFactory(typeof(FlashlightItem))]
internal sealed class Flashlight(FlashlightItem itemBase) : Item(itemBase), IFlashlight
{
    /// <inheritdoc />
    public new UnityObjectWrapper<FlashlightItem> Base { get; } = itemBase;

    /// <inheritdoc />
    public bool IsActive
    {
        get => Base.Instance.IsEmittingLight;
        set
        {
            Base.Instance.IsEmittingLight = value;
            new FlashlightNetworkHandler.FlashlightMessage(Serial, value).SendToAuthenticated();
        }
    }
}