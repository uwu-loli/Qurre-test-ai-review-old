using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using UnityEngine;
using HazardBase = Hazards.EnvironmentalHazard;

namespace Qurre.API.Entities.Hazards;

[PublicAPI]
public interface IHazard : IEntity
{
    #region Properties

    UnityObjectWrapper<HazardBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsActive { get; }

    /// <exception cref="ObjectDestroyedException" />
    float MaxDistance { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float MaxHeightDistance { get; set; }

    #endregion

    #region Methods

    /// <exception cref="ObjectDestroyedException" />
    bool IsInArea(Vector3 worldPoint);

    /// <exception cref="ObjectDestroyedException" />
    bool IsInArea(IReadOnlyTransformEntity entity);

    #endregion
}