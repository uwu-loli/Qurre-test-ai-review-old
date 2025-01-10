using System;
using AdminToys;
using JetBrains.Annotations;
using Qurre.API.Interfaces;
using UnityEngine;

namespace Qurre.API.Controllers.Components;

[PublicAPI]
public abstract class ToyEntity<TBaseToy, T> : NetworkEntity<TBaseToy, T>, IToyEntity
    where TBaseToy : AdminToyBase
    where T : ToyEntity<TBaseToy, T>
{
    public event Action? IsStaticUpdated;
    public event Action? MovementSmoothingUpdated;
    
    /// <summary>
    /// 
    /// </summary>
    public bool IsStatic
    {
        get => Base.NetworkIsStatic;
        set
        {
            Base.NetworkIsStatic = value;
            IsStaticUpdated?.Invoke();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public byte MovementSmoothing
    {
        get => Base.NetworkMovementSmoothing;
        set
        {
            Base.NetworkMovementSmoothing = value;
            MovementSmoothingUpdated?.Invoke();
        }
    }
    
    /// <inheritdoc />
    public sealed override Vector3 WorldPosition
    {
        get => base.WorldPosition;
        set
        {
            if (IsStatic) return;
            base.WorldPosition = value;
        }
    }

    /// <inheritdoc />
    public sealed override Quaternion WorldRotation
    {
        get => base.WorldRotation;
        set
        {
            if (IsStatic) return;
            base.WorldRotation = value;
        }
    }

    /// <inheritdoc />
    public sealed override Vector3 WorldScale
    {
        get => base.WorldScale;
        set
        {
            if (IsStatic) return;
            base.WorldScale = value;
        }
    }
    
    /// <inheritdoc />
    public sealed override Vector3 LocalPosition
    {
        get => base.LocalPosition;
        set
        {
            if (IsStatic) return;
            base.LocalPosition = value;
        }
    }

    /// <inheritdoc />
    public sealed override Quaternion LocalRotation
    {
        get => base.LocalRotation;
        set
        {
            if (IsStatic) return;
            base.LocalRotation = value;
        }
    }

    /// <inheritdoc />
    public sealed override Vector3 LocalScale
    {
        get => base.LocalScale;
        set
        {
            if (IsStatic) return;
            base.LocalScale = value;
        }
    }

    protected sealed override void OnPositionUpdated() => Base.NetworkPosition = WorldPosition;
    protected sealed override void OnRotationUpdated() => Base.NetworkRotation = WorldRotation;
    protected sealed override void OnScaleUpdated() => Base.NetworkScale = WorldScale;
}
