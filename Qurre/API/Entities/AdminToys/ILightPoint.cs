using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using UnityEngine;
using LightPointBase = AdminToys.LightSourceToy;

namespace Qurre.API.Entities.AdminToys;

[PublicAPI]
public interface ILightPoint : IAdminToy
{
    #region Properties

    new UnityObjectWrapper<LightPointBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    Color Color { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float Intensity { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float Range { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    LightType LightType { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    LightShadows ShadowType { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float ShadowStrength { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    LightShape ShapeType { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float SpotAngle { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float SpotInnerAngle { get; set; }

    #endregion
}