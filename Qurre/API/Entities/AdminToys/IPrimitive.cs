using AdminToys;
using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using UnityEngine;
using PrimitiveBase = AdminToys.PrimitiveObjectToy;

namespace Qurre.API.Entities.AdminToys;

[PublicAPI]
public interface IPrimitive : IAdminToy
{
    #region Properties

    new UnityObjectWrapper<PrimitiveBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    PrimitiveType PrimitiveType { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    Color MaterialColor { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    PrimitiveFlags PrimitiveFlags { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsCollidable { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsVisible { get; set; }

    #endregion
}