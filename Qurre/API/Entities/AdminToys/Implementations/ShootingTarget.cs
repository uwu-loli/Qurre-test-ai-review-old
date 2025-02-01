using System;
using Qurre.API.Addons;
using Qurre.API.Core;
using Qurre.API.Enums;
using Qurre.Internal.Attributes;
using ShootingTargetBase = AdminToys.ShootingTarget;

namespace Qurre.API.Entities.AdminToys.Implementations;

[EntityWrapBindForFactory(typeof(ShootingTargetBase))]
internal sealed class ShootingTarget : AdminToy, IShootingTarget
{
    /// <inheritdoc />
    public new UnityObjectWrapper<ShootingTargetBase> Base { get; }

    /// <inheritdoc />
    public ShootingTargetPrefabs PrefabType { get; }

    public ShootingTarget(ShootingTargetBase shootingTargetBase) : base(shootingTargetBase)
    {
        Base = shootingTargetBase;
        
        PrefabType = NetworkIdentity.Instance.assetId switch
        {
            Prefabs.AssetIdShootingTargetSport => ShootingTargetPrefabs.Sport,
            Prefabs.AssetIdShootingTargetDBoy => ShootingTargetPrefabs.Dboy,
            Prefabs.AssetIdShootingTargetBinary => ShootingTargetPrefabs.Binary,
            _ => ShootingTargetPrefabs.Unknown
        };
    }

    /// <inheritdoc />
    public void Clear()
    {
        Base.Instance.ClearTarget();
    }
}