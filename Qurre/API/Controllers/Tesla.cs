using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Mirror;
using PlayerRoles;
using Qurre.API.Controllers.Components;
using Qurre.API.Exceptions;
using UnityEngine;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Tesla : GeneratedNetworkEntity<TeslaGate, Tesla>
{
    protected override TeslaGate UnsafeBase { get; }

    public bool Enable { get; set; } = true;
    public bool Allow079Interact { get; set; } = true;

    public List<RoleTypeId> ImmunityRoles { get; } = [];
    public List<Player> ImmunityPlayers { get; } = [];

    public Vector3 SizeOfKiller
    {
        get => Base.sizeOfKiller;
        set => Base.sizeOfKiller = value;
    }

    public bool InProgress
    {
        get => Base.InProgress;
        set => Base.InProgress = value;
    }

    public float SizeOfTrigger
    {
        get => Base.sizeOfTrigger;
        set => Base.sizeOfTrigger = value;
    }

    private Tesla(TeslaGate teslaBase)
    {
        UnsafeBase = teslaBase;
        ScaleUpdated += () => SizeOfKiller = WorldScale;

        BaseToWrap[Base] = this;
        AddEntityLink();
        Destroyed += OnDestroyed;
    }

    public void Trigger(bool instant = false)
    {
        if (instant) Base.RpcInstantBurst();
        else Base.RpcPlayAnimation();
    }

    private void OnDestroyed()
    {
        if (!BaseToWrap.ContainsKey(Base)) return;
        BaseToWrap.Remove(Base);
    }

    public static Tesla? Get(TeslaGate teslaBase)
    {
        if (!teslaBase) return null;
        return BaseToWrap.TryGetValue(teslaBase, out var tesla) ? tesla : new Tesla(teslaBase);
    }

    public static bool TryGet(TeslaGate teslaBase, [NotNullWhen(true)] out Tesla? tesla)
    {
        tesla = Get(teslaBase);
        return tesla is not null;
    }
}