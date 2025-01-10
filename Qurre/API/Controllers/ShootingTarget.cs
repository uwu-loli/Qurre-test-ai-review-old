using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AdminToys;
using JetBrains.Annotations;
using Mirror;
using Qurre.API.Controllers.Components;
using Qurre.API.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Controllers;

[PublicAPI]
public class ShootingTarget : ToyEntity<AdminToys.ShootingTarget, ShootingTarget>
{
    /// <inheritdoc />
    protected sealed override AdminToys.ShootingTarget UnsafeBase { get; }

    /// <summary>
    /// 
    /// </summary>
    public TargetPrefabs PrefabType { get; }

    public ShootingTarget(TargetPrefabs type, Vector3 position, Quaternion rotation = default, Vector3 size = default)
    {
        if (!type.GetPrefab().TryGetComponent(out AdminToyBase primitiveToyBase))
            throw new ArgumentNullException(nameof(primitiveToyBase));

        var instanceToyBase = Object.Instantiate(primitiveToyBase, position, rotation);

        PrefabType = type;
        UnsafeBase = (AdminToys.ShootingTarget)instanceToyBase;
        Base.transform.localScale = size == default ? Vector3.one : size;
        NetworkServer.Spawn(Base.gameObject);

        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    private ShootingTarget(AdminToys.ShootingTarget targetBase)
    {
        try
        {
            PrefabType = (TargetPrefabs)Enum.Parse(typeof(TargetPrefabs), targetBase._targetName);
        }
        catch
        {
            PrefabType = TargetPrefabs.Binary;
        }

        UnsafeBase = targetBase;

        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    public void Clear()
    {
        Base.ClearTarget();
    }

    public static ShootingTarget? Get(AdminToys.ShootingTarget targetBase)
    {
        if (!targetBase) return null;
        return BaseToWrap.TryGetValue(targetBase, out var target) ? target : new ShootingTarget(targetBase);
    }

    public static bool TryGet(AdminToys.ShootingTarget targetBase, [NotNullWhen(true)] out ShootingTarget? target)
    {
        target = Get(targetBase);
        return target is not null;
    }
}