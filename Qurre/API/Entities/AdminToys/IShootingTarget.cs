using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Enums;
using Qurre.API.Exceptions;
using ShootingTargetBase = AdminToys.ShootingTarget;

namespace Qurre.API.Entities.AdminToys;

[PublicAPI]
public interface IShootingTarget : IAdminToy
{
    #region Methods

    /// <exception cref="ObjectDestroyedException" />
    void Clear();

    #endregion

    #region Properties

    new UnityObjectWrapper<ShootingTargetBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    ShootingTargetPrefabs PrefabType { get; }

    #endregion
}