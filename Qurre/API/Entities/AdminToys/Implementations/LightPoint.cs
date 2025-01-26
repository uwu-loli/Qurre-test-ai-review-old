using Qurre.API.Core;
using Qurre.Internal.Attributes;
using UnityEngine;
using LightPointBase = AdminToys.LightSourceToy;

namespace Qurre.API.Entities.AdminToys.Implementations;

[EntityWrapBindForFactory(typeof(LightPointBase))]
internal sealed class LightPoint(LightPointBase lightPointBase) : AdminToy(lightPointBase), ILightPoint
{
    /// <inheritdoc />
    public new UnityObjectWrapper<LightPointBase> Base { get; } = lightPointBase;

    /// <inheritdoc />
    public Color Color
    {
        get => Base.Instance.NetworkLightColor;
        set => Base.Instance.NetworkLightColor = value;
    }

    /// <inheritdoc />
    public float Intensity
    {
        get => Base.Instance.NetworkLightIntensity;
        set => Base.Instance.NetworkLightIntensity = value;
    }

    /// <inheritdoc />
    public float Range
    {
        get => Base.Instance.NetworkLightRange;
        set => Base.Instance.NetworkLightRange = value;
    }

    /// <inheritdoc />
    public LightType LightType
    {
        get => Base.Instance.NetworkLightType;
        set => Base.Instance.NetworkLightType = value;
    }

    /// <inheritdoc />
    public LightShadows ShadowType
    {
        get => Base.Instance.NetworkShadowType;
        set => Base.Instance.NetworkShadowType = value;
    }

    /// <inheritdoc />
    public float ShadowStrength
    {
        get => Base.Instance.NetworkShadowStrength;
        set => Base.Instance.NetworkShadowStrength = value;
    }

    /// <inheritdoc />
    public LightShape ShapeType
    {
        get => Base.Instance.NetworkLightShape;
        set => Base.Instance.NetworkLightShape = value;
    }

    /// <inheritdoc />
    public float SpotAngle
    {
        get => Base.Instance.NetworkSpotAngle;
        set => Base.Instance.NetworkSpotAngle = value;
    }

    /// <inheritdoc />
    public float SpotInnerAngle
    {
        get => Base.Instance.NetworkInnerSpotAngle;
        set => Base.Instance.NetworkInnerSpotAngle = value;
    }
}