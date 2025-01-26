using JetBrains.Annotations;
using Qurre.API.Exceptions;
using UnityEngine;

namespace Qurre.API.Core.Implementations;

[MeansImplicitUse(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal abstract class ReadOnlyTransformEntity(GameObject gameObject) : Entity(gameObject), IReadOnlyTransformEntity
{
    /// <exception cref="ObjectDestroyedException" />
    private UnityObjectWrapper<Transform> Transform { get; } = gameObject.transform;

    /// <inheritdoc />
    public Vector3 WorldPosition => Transform.Instance.position;

    /// <inheritdoc />
    public Quaternion WorldRotation => Transform.Instance.rotation;

    /// <inheritdoc />
    public Vector3 WorldEulerAngles => WorldRotation.eulerAngles;

    /// <inheritdoc />
    public Vector3 WorldScale => Transform.Instance.lossyScale;

    /// <inheritdoc />
    public Vector3 LocalPosition => Transform.Instance.localPosition;

    /// <inheritdoc />
    public Quaternion LocalRotation => Transform.Instance.localRotation;

    /// <inheritdoc />
    public Vector3 LocalEulerAngles => LocalRotation.eulerAngles;

    /// <inheritdoc />
    public Vector3 LocalScale => Transform.Instance.localScale;
}