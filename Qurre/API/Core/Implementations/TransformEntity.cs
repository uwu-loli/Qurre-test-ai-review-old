using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Qurre.API.Core.Implementations;

[MeansImplicitUse(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal abstract class TransformEntity(GameObject gameObject) : Entity(gameObject), ITransformEntity
{
    private UnityObjectWrapper<Transform> Transform { get; } = gameObject.transform;

    /// <inheritdoc />
    public event Action? PositionChanged;

    /// <inheritdoc />
    public event Action? RotationChanged;

    /// <inheritdoc />
    public event Action? ScaleChanged;

    protected virtual bool CanModifyTransform { get; } = true;

    /// <inheritdoc cref="ITransformEntity.WorldPosition" />
    public Vector3 WorldPosition
    {
        get => Transform.Instance.position;
        set
        {
            if (!CanModifyTransform) return;
            Transform.Instance.position = value; 
            PositionChanged?.Invoke();
        }
    }

    /// <inheritdoc cref="ITransformEntity.WorldRotation" />
    public Quaternion WorldRotation
    {
        get => Transform.Instance.rotation;
        set
        {
            if (!CanModifyTransform) return;
            Transform.Instance.rotation = value; 
            RotationChanged?.Invoke();
        }
    }

    /// <inheritdoc cref="ITransformEntity.WorldEulerAngles" />
    public Vector3 WorldEulerAngles
    {
        get => WorldRotation.eulerAngles;
        set => WorldRotation = Quaternion.Euler(value);
    }

    /// <inheritdoc cref="ITransformEntity.WorldScale" />
    public Vector3 WorldScale
    {
        get => Transform.Instance.lossyScale;
        set
        {
            if (!CanModifyTransform) return;
            var parentScale = Transform.Instance.lossyScale.SafeUnscale(Transform.Instance.localScale);
            Transform.Instance.localScale = value.SafeUnscale(parentScale);
            ScaleChanged?.Invoke();
        }
    }

    /// <inheritdoc cref="ITransformEntity.LocalPosition" />
    public Vector3 LocalPosition
    {
        get => Transform.Instance.localPosition;
        set
        {
            if (!CanModifyTransform) return;
            Transform.Instance.localPosition = value;
            PositionChanged?.Invoke();
        }
    }

    /// <inheritdoc cref="ITransformEntity.LocalRotation" />
    public Quaternion LocalRotation
    {
        get => Transform.Instance.localRotation;
        set
        {
            if (!CanModifyTransform) return;
            Transform.Instance.localRotation = value;
            RotationChanged?.Invoke();
        }
    }

    /// <inheritdoc cref="ITransformEntity.LocalEulerAngles" />
    public Vector3 LocalEulerAngles
    {
        get => LocalRotation.eulerAngles;
        set => LocalRotation = Quaternion.Euler(value);
    }

    /// <inheritdoc cref="ITransformEntity.LocalScale" />
    public Vector3 LocalScale
    {
        get => Transform.Instance.localScale;
        set
        {
            if (!CanModifyTransform) return;
            Transform.Instance.localScale = value;
            ScaleChanged?.Invoke();
        }
    }
}