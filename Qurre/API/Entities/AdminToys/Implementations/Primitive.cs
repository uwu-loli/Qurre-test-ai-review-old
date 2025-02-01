using AdminToys;
using Qurre.API.Core;
using Qurre.Internal.Attributes;
using UnityEngine;
using PrimitiveBase = AdminToys.PrimitiveObjectToy;

namespace Qurre.API.Entities.AdminToys.Implementations;

[EntityWrapBindForFactory(typeof(PrimitiveBase))]
internal sealed class Primitive(PrimitiveBase primitiveBase) : AdminToy(primitiveBase), IPrimitive
{
    /// <inheritdoc />
    public new UnityObjectWrapper<PrimitiveBase> Base { get; } = primitiveBase;

    /// <inheritdoc />
    public PrimitiveType PrimitiveType
    {
        get => Base.Instance.NetworkPrimitiveType;
        set => Base.Instance.NetworkPrimitiveType = value;
    }

    /// <inheritdoc />
    public Color MaterialColor
    {
        get => Base.Instance.NetworkMaterialColor;
        set => Base.Instance.NetworkMaterialColor = value;
    }

    /// <inheritdoc />
    public PrimitiveFlags PrimitiveFlags
    {
        get => Base.Instance.NetworkPrimitiveFlags;
        set => Base.Instance.NetworkPrimitiveFlags = value;
    }

    /// <inheritdoc />
    public bool IsCollidable
    {
        get => PrimitiveFlags.HasFlag(PrimitiveFlags.Visible);
        set
        {
            if (value) PrimitiveFlags |= PrimitiveFlags.Visible;
            else PrimitiveFlags &= ~PrimitiveFlags.Visible;
        }
    }

    /// <inheritdoc />
    public bool IsVisible
    {
        get => Base.Instance.PrimitiveFlags.HasFlag(PrimitiveFlags.Visible);
        set
        {
            if (value) PrimitiveFlags |= PrimitiveFlags.Visible;
            else PrimitiveFlags &= ~PrimitiveFlags.Visible;
        }
    }
}