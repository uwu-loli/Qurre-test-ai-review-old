using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.Internal.Attributes;
using UnityEngine;
using HazardBase = Hazards.EnvironmentalHazard;

namespace Qurre.API.Entities.Hazards.Implementations;

[EntityWrapBindForFactory(typeof(HazardBase))]
internal abstract class Hazard(HazardBase hazardBase) : NetworkEntity(hazardBase.gameObject), IHazard
{
    /// <inheritdoc />
    public UnityObjectWrapper<HazardBase> Base { get; } = hazardBase;

    /// <inheritdoc />
    public bool IsActive => Base.Instance.IsActive;

    /// <inheritdoc />
    public float MaxDistance
    {
        get => Base.Instance.MaxDistance;
        set => Base.Instance.MaxDistance = value;
    }

    /// <inheritdoc />
    public float MaxHeightDistance
    {
        get => Base.Instance.MaxHeightDistance;
        set => Base.Instance.MaxHeightDistance = value;
    }

    /// <inheritdoc />
    public bool IsInArea(Vector3 worldPoint)
    {
        return Base.Instance.IsInArea(Base.Instance.SourcePosition, worldPoint);
    }

    /// <inheritdoc />
    public bool IsInArea(IReadOnlyTransformEntity entity)
    {
        return entity.GameObject.IsAlive && IsInArea(entity.WorldPosition);
    }
}