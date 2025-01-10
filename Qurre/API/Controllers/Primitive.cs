using System;
using System.Diagnostics.CodeAnalysis;
using AdminToys;
using Footprinting;
using JetBrains.Annotations;
using Mirror;
using Qurre.API.Addons;
using Qurre.API.Controllers.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Primitive : ToyEntity<PrimitiveObjectToy, Primitive>
{
    protected sealed override PrimitiveObjectToy UnsafeBase { get; }

    private Primitive(PrimitiveObjectToy primitiveBase)
    {
        UnsafeBase = primitiveBase;
        BaseToWrap[Base] = this;
        AddEntityLink();
    }
    
    public Primitive(PrimitiveType type, Vector3 position = default, Color color = default,
        Quaternion rotation = default, Vector3 size = default, bool collider = true,
        bool visible = true)
    {
        if (Prefabs.Primitive == null)
            throw new NullReferenceException(nameof(Prefabs.Primitive));

        if (!Prefabs.Primitive.TryGetComponent<PrimitiveObjectToy>(out PrimitiveObjectToy? primitiveToyBase))
            throw new NullReferenceException("PrimitiveObjectToy not found");

        UnsafeBase = Object.Instantiate(primitiveToyBase);
        Base.SpawnerFootprint = new Footprint(Server.Host.ReferenceHub);
        NetworkServer.Spawn(Base.gameObject);

        WorldPosition = position;
        WorldRotation = rotation;
        WorldScale = size == default ? Vector3.one : size;

        Type = type;
        Color = color == default ? Color.white : color;
        Collider = collider;
        Visible = visible;

        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    public Color Color
    {
        get => Base.NetworkMaterialColor;
        set => Base.NetworkMaterialColor = value;
    }

    public PrimitiveType Type
    {
        get => Base.NetworkPrimitiveType;
        set => Base.NetworkPrimitiveType = value;
    }

    public PrimitiveFlags Flags
    {
        get => Base.NetworkPrimitiveFlags;
        set => Base.NetworkPrimitiveFlags = value;
    }

    public bool Collider
    {
        get => Flags.HasFlag(PrimitiveFlags.Collidable);
        set
        {
            if (!value)
                Flags &= ~PrimitiveFlags.Collidable;
            else
                Flags |= PrimitiveFlags.Collidable;
        }
    }

    public bool Visible
    {
        get => Flags.HasFlag(PrimitiveFlags.Visible);
        set
        {
            if (!value)
                Flags &= ~PrimitiveFlags.Visible;
            else
                Flags |= PrimitiveFlags.Visible;
        }
    }

    public static Primitive? Get(PrimitiveObjectToy primitiveBase)
    {
        if (!primitiveBase) return null;
        return BaseToWrap.TryGetValue(primitiveBase, out var primitive) ? primitive : new Primitive(primitiveBase);
    }

    public static bool TryGet(PrimitiveObjectToy primitiveBase, [NotNullWhen(true)] out Primitive? primitive)
    {
        primitive = Get(primitiveBase);
        return primitive is not null;
    }
}