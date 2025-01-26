using InventorySystem.Items.Radio;
using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Enums;

namespace Qurre.API.Entities.Items;

[PublicAPI]
public interface IRadio : IItem
{
    new UnityObjectWrapper<RadioItem> Base { get; }

    float Battery { get; set; }
    byte BatteryPercent { get; set; }
    RadioStatus Status { get; set; }
    RadioRangeMode StatusSettings { get; set; }
}