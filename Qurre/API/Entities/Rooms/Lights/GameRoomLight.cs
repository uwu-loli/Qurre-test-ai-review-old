using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using UnityEngine;

namespace Qurre.API.Entities.Rooms.Lights;

internal class GameRoomLight(RoomLightController roomLightBase) : Entity(roomLightBase.gameObject), IGameRoomLight
{
    private Color _color;
    private float _lightIntensity = 1.0F;

    /// <inheritdoc />
    public UnityObjectWrapper<RoomLightController> Base { get; } = roomLightBase;

    /// <inheritdoc />
    public Color DefaultColor { get; set; } = roomLightBase.NetworkOverrideColor;

    /// <inheritdoc />
    public float DisabledTimer
    {
        get => Base.Instance._flickerDuration;
        set => Disable(value);
    }

    /// <inheritdoc />
    public bool IsEnabled
    {
        get => Base.Instance.NetworkLightsEnabled;
        set => Base.Instance.NetworkLightsEnabled = value;
    }

    /// <inheritdoc />
    public Color Color
    {
        get => _color;
        set
        {
            Base.Instance.NetworkOverrideColor = value * _lightIntensity;
            _color = value;
        }
    }

    /// <inheritdoc />
    public float Intensity
    {
        get => _lightIntensity;
        set
        {
            _lightIntensity = value;
            Color = _color * _lightIntensity;
        }
    }

    /// <inheritdoc />
    public Color RealColor
    {
        get => Base.Instance.NetworkOverrideColor;
        set
        {
            _lightIntensity = 1.0F;
            Color = value;
        }
    }

    /// <inheritdoc />
    public void Disable(float? duration = null)
    {
        if (duration is null)
        {
            IsEnabled = false;
            return;
        }

        DisabledTimer = duration.Value;
    }

    /// <inheritdoc />
    public void Reset()
    {
        IsEnabled = true;
        RealColor = DefaultColor;
    }
}