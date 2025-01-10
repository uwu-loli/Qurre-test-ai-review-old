using System;
using JetBrains.Annotations;
using Qurre.API.Interfaces;
using UnityEngine;

namespace Qurre.API.Controllers.Components;

[PublicAPI]
public abstract class TransformableEntity<TBase, T> : Entity<TBase, T>, ITransformableEntity
    where TBase : MonoBehaviour
    where T : TransformableEntity<TBase, T>
{
    public event Action? PositionUpdated;
    public event Action? RotationUpdated;
    public event Action? ScaleUpdated;

    public Transform Transform => GameObject.transform;

    public virtual Vector3 WorldPosition
    {
        get => Transform.position;
        set
        {
            Transform.position = value;
            InvokePositionUpdated();
        }
    }

    public virtual Quaternion WorldRotation
    {
        get => Transform.rotation;
        set
        {
            Transform.rotation = value;
            InvokeRotationUpdated();
        }
    }

    public virtual Vector3 WorldRotationEuler
    {
        get => WorldRotation.eulerAngles;
        set => WorldRotation = Quaternion.Euler(value);
    }

    public virtual Vector3 WorldScale
    {
        get => Transform.lossyScale;
        set
        {
            var parentScale = Transform.lossyScale.SafeUnscale(Transform.localScale);
            Transform.localScale = value.SafeUnscale(parentScale);
            InvokeScaleUpdated();
        }
    }
    
    public virtual Vector3 LocalPosition
    {
        get => Transform.localPosition;
        set
        {
            Transform.localPosition = value;
            InvokePositionUpdated();
        }
    }

    private Quaternion _lastLocalRotation;
    public virtual Quaternion LocalRotation
    {
        get => Transform.localRotation;
        set
        {
            if (_lastLocalRotation == value) return;
            Transform.localRotation = value;
            _lastLocalRotation = value;
            InvokeRotationUpdated();
        }
    }

    public virtual Vector3 LocalRotationEuler
    {
        get => LocalRotation.eulerAngles;
        set => LocalRotation = Quaternion.Euler(value);
    }

    public virtual Vector3 LocalScale
    {
        get => Transform.localScale;
        set
        {
            Transform.localScale = value;
            InvokeScaleUpdated();
        }
    }

    private void InvokePositionUpdated() => PositionUpdated?.Invoke();
    private void InvokeRotationUpdated() => RotationUpdated?.Invoke();
    private void InvokeScaleUpdated() => ScaleUpdated?.Invoke();
}
