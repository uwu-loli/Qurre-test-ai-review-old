using System;
using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using AdminToyBase = AdminToys.AdminToyBase;

namespace Qurre.API.Entities.AdminToys;

[PublicAPI]
public interface IAdminToy : INetworkEntity
{
    #region Properties

    event Action IsStaticChanged;

    event Action SmoothingChanged;

    UnityObjectWrapper<AdminToyBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsStatic { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    byte Smoothing { get; set; }

    #endregion
}