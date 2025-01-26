using Interactables.Interobjects;
using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Entities.Environment;
using Qurre.API.Exceptions;
using UnityEngine;
using ElevatorDoorBase = Interactables.Interobjects.ElevatorDoor;

namespace Qurre.API.Entities.Doors;

[PublicAPI]
public interface IElevatorDoor : IDoor
{
    #region Properties

    new UnityObjectWrapper<ElevatorDoorBase> Base { get; }

    ILift? Lift { get; }

    /// <exception cref="ObjectDestroyedException" />
    ElevatorGroup AssignedGroup { get; }

    /// <exception cref="ObjectDestroyedException" />
    Vector3 TargetPosition { get; }

    /// <exception cref="ObjectDestroyedException" />
    Vector3 TopPosition { get; }

    /// <exception cref="ObjectDestroyedException" />
    Vector3 BottomPosition { get; }

    #endregion
}