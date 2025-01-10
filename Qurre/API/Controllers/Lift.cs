using System.Diagnostics.CodeAnalysis;
using Interactables.Interobjects;
using JetBrains.Annotations;
using Qurre.API.Controllers.Components;
using UnityEngine;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Lift : NetworkEntity<ElevatorChamber, Lift>
{
    protected override ElevatorChamber UnsafeBase { get; }

    private Lift(ElevatorChamber elevator)
    {
        UnsafeBase = elevator;

        BaseToWrap[Base] = this;
        AddEntityLink();
        Destroyed += OnDestroyed;
    }

    public ElevatorGroup Type => Base.AssignedGroup;

    public Bounds Bounds => Base.WorldspaceBounds;

    public ElevatorChamber.ElevatorSequence Status
    {
        get => Base.CurSequence;
        set => Base.CurSequence = value;
    }

    public void Use()
    {
        Status = ElevatorChamber.ElevatorSequence.Ready;
    }

    private void OnDestroyed()
    {
        BaseToWrap.Remove(Base);
    }

    public static Lift? Get(ElevatorChamber liftBase)
    {
        if (!liftBase) return null;
        return BaseToWrap.TryGetValue(liftBase, out var lift) ? lift : new Lift(liftBase);
    }

    public static bool TryGet(ElevatorChamber liftBase, [NotNullWhen(true)] out Lift? lift)
    {
        lift = Get(liftBase);
        return lift is not null;
    }
}