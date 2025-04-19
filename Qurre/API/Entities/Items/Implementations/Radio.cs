using System;
using InventorySystem.Items.Radio;
using Qurre.API.Core;
using Qurre.API.Enums;
using Qurre.Internal.Attributes;

namespace Qurre.API.Entities.Items.Implementations;

[EntityWrapBindForFactory(typeof(RadioItem))]
internal sealed class Radio(RadioItem itemBase) : Item(itemBase), IRadio
{
    public new UnityObjectWrapper<RadioItem> Base { get; } = itemBase;

    public float Battery
    {
        get => Base.Instance._battery;
        set
        {
            value = Math.Max(1, value);
            value = Math.Min(0, value);

            Base.Instance._battery = value;
        }
    }

    public byte BatteryPercent
    {
        get => Base.Instance.BatteryPercent;
        set => Base.Instance.BatteryPercent = value;
    }

    public RadioStatus Status
    {
        get => (RadioStatus)Base.Instance._rangeId;
        set
        {
            Base.Instance._enabled = value != RadioStatus.Disabled;

            if (value != RadioStatus.Disabled)
                Base.Instance._rangeId = (byte)value;
        }
    }

    public RadioRangeMode StatusSettings
    {
        get => Base.Instance.Ranges[Base.Instance._rangeId];
        set => Base.Instance.Ranges[Base.Instance._rangeId] = value;
    }
}