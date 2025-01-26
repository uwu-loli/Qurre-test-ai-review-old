using System;
using Footprinting;
using MapGeneration.Distributors;
using Qurre.API.Core;
using Qurre.Internal.Attributes;
using UnityEngine;

namespace Qurre.API.Entities.Structures.Implementations;

[EntityWrapBindForFactory(typeof(Scp079Generator))]
internal sealed class Generator(Scp079Generator generatorBase) : StructureEntity(generatorBase), IGenerator
{
    /// <inheritdoc />
    public new UnityObjectWrapper<Scp079Generator> Base { get; } = generatorBase;

    /// <inheritdoc />
    public float ActivationTime => Base.Instance._totalActivationTime;

    /// <inheritdoc />
    public bool IsOpen
    {
        get => Base.Instance.IsOpen;
        set => Base.Instance.IsOpen = value;
    }

    /// <inheritdoc />
    public bool IsLocked
    {
        get => !Base.Instance.IsUnlocked;
        set => Base.Instance.IsUnlocked = !value;
    }

    /// <inheritdoc />
    public bool IsActive
    {
        get => Base.Instance.Activating;
        set
        {
            Base.Instance.Activating = value;
            if (value) Base.Instance._leverStopwatch.Restart();
            Base.Instance._lastActivator = new Footprint(Server.Host.ReferenceHub);
            Base.Instance._targetCooldown = Base.Instance._doorToggleCooldownTime;
        }
    }

    /// <inheritdoc />
    public bool IsActivating
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool IsDeactivating
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool IsEngaged
    {
        get => Base.Instance.Engaged;
        set => Base.Instance.Engaged = value;
    }

    /// <inheritdoc />
    public short RemainingTime
    {
        get => Base.Instance._syncTime;
        set => Base.Instance._syncTime = value;
    }

    /// <inheritdoc />
    public float DeniedCooldownTime
    {
        get => Base.Instance._deniedCooldownTime;
        set => Base.Instance._deniedCooldownTime = value;
    }

    /// <inheritdoc />
    public float UnlockCooldownTime
    {
        get => Base.Instance._unlockCooldownTime;
        set => Base.Instance._unlockCooldownTime = value;
    }

    /// <inheritdoc />
    public AudioClip DeniedClip => Base.Instance._deniedClip;

    /// <inheritdoc />
    public AudioClip OpenClip => Base.Instance._openClip;

    /// <inheritdoc />
    public AudioClip CloseClip => Base.Instance._closeClip;

    /// <inheritdoc />
    public AudioClip CountdownClip => Base.Instance._countdownClip;
}