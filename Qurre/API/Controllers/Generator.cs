using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using MapGeneration.Distributors;
using Mirror;
using Qurre.API.Addons;
using Qurre.API.Controllers.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Generator : NetworkEntity<Scp079Generator, Generator>
{
    protected override Scp079Generator UnsafeBase { get; }
    private readonly StructurePositionSync _positionSync;

    internal Generator(Scp079Generator g)
    {
        UnsafeBase = g;
        _positionSync = Base.GetComponent<StructurePositionSync>();
        SetupActions();

        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    public Generator(Vector3 position, Quaternion? rotation = null)
    {
        if (Prefabs.Generator == null)
            throw new NullReferenceException(nameof(Prefabs.Generator));

        UnsafeBase = Object.Instantiate(Prefabs.Generator);

        Transform.position = position;
        Transform.rotation = rotation ?? new Quaternion();

        _positionSync = Base.GetComponent<StructurePositionSync>();

        SetupActions();
        NetworkServer.Spawn(Base.gameObject);

        Spawn();

        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    public bool Open
    {
        get => Base.HasFlag(Base._flags, Scp079Generator.GeneratorFlags.Open);
        set
        {
            Base.ServerSetFlag(Scp079Generator.GeneratorFlags.Open, value);
            Base._targetCooldown = Base._doorToggleCooldownTime;
        }
    }

    public bool Lock
    {
        get => !Base.HasFlag(Base._flags, Scp079Generator.GeneratorFlags.Unlocked);
        set
        {
            Base.ServerSetFlag(Scp079Generator.GeneratorFlags.Unlocked, !value);
            Base._targetCooldown = Base._unlockCooldownTime;
        }
    }

    public bool Active
    {
        get => Base.Activating;
        set
        {
            Base.Activating = value;
            if (value) Base._leverStopwatch.Restart();
            Base._targetCooldown = Base._doorToggleCooldownTime;
        }
    }

    public bool Engaged
    {
        get => Base.Engaged;
        set => Base.Engaged = value;
    }

    public short Time
    {
        get => Base._syncTime;
        set => Base.Network_syncTime = value;
    }

    private void SetupActions()
    {
        PositionUpdated += () => _positionSync.Network_position = WorldPosition;
        RotationUpdated += () => _positionSync.Network_rotationY = (sbyte)(WorldRotationEuler.y / 5.625f);
    }

    public static Generator? Get(Scp079Generator generatorBase)
    {
        if (!generatorBase) return null;
        return BaseToWrap.TryGetValue(generatorBase, out var generator) ? generator : new Generator(generatorBase);
    }

    public static bool TryGet(Scp079Generator generatorBase, [NotNullWhen(true)] out Generator? generator)
    {
        generator = Get(generatorBase);
        return generator is not null;
    }
}