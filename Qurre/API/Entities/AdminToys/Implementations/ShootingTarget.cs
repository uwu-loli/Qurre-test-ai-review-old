using System;
using Qurre.API.Core;
using Qurre.API.Enums;
using Qurre.Internal.Attributes;
using ShootingTargetBase = AdminToys.ShootingTarget;

namespace Qurre.API.Entities.AdminToys.Implementations;

[EntityWrapBindForFactory(typeof(ShootingTargetBase))]
internal sealed class ShootingTarget(ShootingTargetBase shootingTargetBase)
    : AdminToy(shootingTargetBase), IShootingTarget
{
    /// <inheritdoc />
    public new UnityObjectWrapper<ShootingTargetBase> Base { get; } = shootingTargetBase;

    /// <inheritdoc />
    public ShootingTargetPrefabs PrefabType => throw new NotImplementedException();

    /// <inheritdoc />
    public void Clear()
    {
        Base.Instance.ClearTarget();
    }
}