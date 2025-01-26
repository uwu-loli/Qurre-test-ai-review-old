using Interactables.Interobjects;
using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.Internal.Attributes;
using UnityEngine;

namespace Qurre.API.Entities.Environment.Implementations;

[EntityWrapBindForFactory(typeof(ElevatorChamber))]
internal sealed class Lift(ElevatorChamber liftBase) : LevelEntity(liftBase.gameObject), ILift
{
    /// <inheritdoc />
    public UnityObjectWrapper<ElevatorChamber> Base { get; } = liftBase;

    /// <inheritdoc />
    public bool IsReady => Base.Instance.IsReadyForUserInput;

    /// <inheritdoc />
    public bool InProgress => !IsReady;

    /// <inheritdoc />
    public int DestinationLevel => Base.Instance.DestinationLevel;

    /// <inheritdoc />
    public int NextLevel => Base.Instance.NextLevel;

    /// <inheritdoc />
    public Bounds WorldBounds => Base.Instance.WorldspaceBounds;

    /// <inheritdoc />
    public ElevatorGroup AssignedGroup
    {
        get => Base.Instance.NetworkAssignedGroup;
        set => Base.Instance.NetworkAssignedGroup = value;
    }

    /// <inheritdoc />
    public ElevatorChamber.ElevatorSequence Status
    {
        get => Base.Instance.CurSequence;
        set => Base.Instance.CurSequence = value;
    }

    /// <inheritdoc />
    public bool IsLocked
    {
        get => Base.Instance.DynamicAdminLock;
        set => Base.Instance.DynamicAdminLock = value;
    }

    /// <inheritdoc />
    public void MoveTo(int level, bool shouldQueue = false)
    {
        Base.Instance.ServerSetDestination(level, shouldQueue);
    }

    /// <inheritdoc />
    public void ForceDestination(int level)
    {
        Base.Instance.ForceDestination(level);
    }

    /// <inheritdoc />
    public void ForceMoveToNextLevel()
    {
        Base.Instance.ServerSetDestination(Base.Instance.NextLevel, false);
    }
}