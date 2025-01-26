using Interactables.Interobjects;
using Qurre.API.Core;
using Qurre.API.Entities.Environment;
using Qurre.API.Utils.Entities;
using Qurre.Internal.Attributes;
using UnityEngine;
using ElevatorDoorBase = Interactables.Interobjects.ElevatorDoor;

namespace Qurre.API.Entities.Doors.Implementations;

[EntityWrapBindForFactory(typeof(ElevatorDoorBase))]
internal sealed class ElevatorDoor(ElevatorDoorBase elevatorDoorBase) : Door(elevatorDoorBase), IElevatorDoor
{
    /// <inheritdoc />
    public new UnityObjectWrapper<ElevatorDoorBase> Base { get; } = elevatorDoorBase;

    /// <inheritdoc />
    public ILift? Lift { get; } = LiftHelper.GetLiftByGroup(elevatorDoorBase.Group);

    /// <inheritdoc />
    public ElevatorGroup AssignedGroup => Base.Instance.Group;

    /// <inheritdoc />
    public Vector3 TargetPosition => Base.Instance.TargetPosition;

    /// <inheritdoc />
    public Vector3 TopPosition => Base.Instance.TopPosition;

    /// <inheritdoc />
    public Vector3 BottomPosition => Base.Instance.BottomPosition;
}