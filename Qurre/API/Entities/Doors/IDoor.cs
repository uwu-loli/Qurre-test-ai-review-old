using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Entities.Rooms;
using Qurre.API.Enums;
using Qurre.API.Exceptions;
using DoorBase = Interactables.Interobjects.DoorUtils.DoorVariant;

namespace Qurre.API.Entities.Doors;

[PublicAPI]
public interface IDoor : ILevelEntity
{
    #region Methods

    /// <exception cref="ObjectDestroyedException" />
    void SetLock(bool state, DoorLockReason reason = DoorLockReason.SpecialDoorFeature);

    #endregion

    #region Properties

    UnityObjectWrapper<DoorBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    DoorTypes DoorType { get; }

    /// <exception cref="ObjectDestroyedException" />
    byte DoorId { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsVisibleThrough { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsCollisionDisabled { get; }

    /// <exception cref="ObjectDestroyedException" />
    DoorVariant.CollisionsDisablingReasons CollisionDisabledReason { get; }

    /// <exception cref="ObjectDestroyedException" />
    float OpenFraction { get; }

    /// <exception cref="ObjectDestroyedException" />
    public bool IsOpenComplete { get; }

    /// <exception cref="ObjectDestroyedException" />
    public bool IsCloseComplete { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsBusy { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsOpen { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsLocked { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    DoorLockReason LockReason { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsRoomsRegistered { get; }

    IReadOnlyCollection<IRoom> Rooms { get; }

    #endregion
}