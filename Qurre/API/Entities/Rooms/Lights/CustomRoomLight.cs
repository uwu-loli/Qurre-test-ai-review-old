using System;
using JetBrains.Annotations;
using MEC;
using Qurre.API.Entities.AdminToys;
using UnityEngine;

namespace Qurre.API.Entities.Rooms.Lights;

[PublicAPI]
public class CustomRoomLight : IRoomLight
{
    private Color _color;
    private CoroutineHandle _disableCoroutineHandle;
    private DateTime? _enableAt;
    private bool _isEnabled;

    /// <summary>
    /// </summary>
    /// <param name="lightPoint"></param>
    public CustomRoomLight(ILightPoint lightPoint)
    {
        Base = lightPoint;
        DefaultColor = RealColor;
        DefaultIntensity = Intensity;
    }

    public ILightPoint Base { get; }

    public Color DefaultColor { get; }

    public float DefaultIntensity { get; }

    /// <inheritdoc />
    public float DisabledTimer
    {
        get => _enableAt is null ? 0 : (float)DateTime.Now.Subtract(_enableAt.Value).TotalSeconds;
        set => Disable(value);
    }

    /// <inheritdoc />
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            RealColor = value ? _color : Color.black;
        }
    }

    /// <inheritdoc />
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            RealColor = IsEnabled ? value : Color.black;
        }
    }

    /// <inheritdoc />
    public float Intensity
    {
        get => Base.Intensity;
        set => Base.Intensity = value;
    }

    /// <inheritdoc />
    public Color RealColor
    {
        get => Base.Color;
        set => Base.Color = value;
    }

    /// <inheritdoc />
    public void Disable(float? duration = null)
    {
        Timing.KillCoroutines(_disableCoroutineHandle);

        if (duration is null)
        {
            IsEnabled = false;
            return;
        }

        IsEnabled = false;
        _enableAt = DateTime.Now.AddSeconds(duration.Value);
        _disableCoroutineHandle = Timing.CallDelayed(duration.Value, () =>
        {
            IsEnabled = true;
            _enableAt = null;
        });
    }

    /// <inheritdoc />
    public void Reset()
    {
        Color = DefaultColor;
        Intensity = DefaultIntensity;
    }
}