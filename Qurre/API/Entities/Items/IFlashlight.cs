using InventorySystem.Items.ToggleableLights.Flashlight;
using JetBrains.Annotations;
using Qurre.API.Core;

namespace Qurre.API.Entities.Items;

[PublicAPI]
public interface IFlashlight : IItem
{
    new UnityObjectWrapper<FlashlightItem> Base { get; }

    bool IsActive { get; set; }
}