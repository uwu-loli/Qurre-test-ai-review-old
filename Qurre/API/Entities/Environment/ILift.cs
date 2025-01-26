using Interactables.Interobjects;
using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using UnityEngine;

namespace Qurre.API.Entities.Environment;

[PublicAPI]
public interface ILift : ILevelEntity
{
    #region Properties

    UnityObjectWrapper<ElevatorChamber> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsReady { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool InProgress { get; }

    /// <exception cref="ObjectDestroyedException" />
    int DestinationLevel { get; }

    /// <exception cref="ObjectDestroyedException" />
    int NextLevel { get; }

    /// <exception cref="ObjectDestroyedException" />
    Bounds WorldBounds { get; }

    /// <exception cref="ObjectDestroyedException" />
    ElevatorGroup AssignedGroup { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    ElevatorChamber.ElevatorSequence Status { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsLocked { get; set; }

    #endregion

    #region Methods

    /// <exception cref="ObjectDestroyedException" />
    void MoveTo(int level, bool shouldQueue = false);

    /// <exception cref="ObjectDestroyedException" />
    void ForceDestination(int level);

    /// <exception cref="ObjectDestroyedException" />
    void ForceMoveToNextLevel();

    #endregion
}