using Qurre.API.Core;
using Qurre.Internal.Attributes;
using UnityEngine;
using TemporaryHazardBase = Hazards.TemporaryHazard;

namespace Qurre.API.Entities.Hazards.Implementations;

[EntityWrapBindForFactory(typeof(TemporaryHazardBase))]
internal abstract class TemporaryHazard(TemporaryHazardBase temporaryHazardBase)
    : Hazard(temporaryHazardBase), ITemporaryHazard
{
    /// <inheritdoc />
    public new UnityObjectWrapper<TemporaryHazardBase> Base { get; } = temporaryHazardBase;

    /// <inheritdoc />
    public float RemainingLifetime => Mathf.Clamp(Lifetime - LifecycleDuration, 0, Lifetime);

    /// <inheritdoc />
    public float LifecycleDuration => Base.Instance._elapsed;

    /// <inheritdoc />
    public float Lifetime => Base.Instance.HazardDuration;

    /// <inheritdoc />
    public float DecaySpeed => Base.Instance.DecaySpeed;
}