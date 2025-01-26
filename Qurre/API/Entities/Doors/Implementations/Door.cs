using System;
using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.API.Entities.Rooms;
using Qurre.API.Enums;
using Qurre.Internal.Attributes;
using RelativePositioning;
using UnityEngine;
using DoorBase = Interactables.Interobjects.DoorUtils.DoorVariant;

namespace Qurre.API.Entities.Doors.Implementations;

[EntityWrapBindForFactory(typeof(DoorBase))]
internal class Door : LevelEntity, IDoor
{
    public Door(DoorBase doorBase) : base(doorBase.gameObject)
    {
        Base = doorBase;
        UnsafeNetIdWaypoint = GameObject.Instance.GetComponent<NetIdWaypoint>();
        // TODO: fix
        //Rooms = Base.Instance.Rooms.Select(EntitiesManager.GetOrException<IGameRoom>).ToList();
    }

    private NetIdWaypoint UnsafeNetIdWaypoint { get; }

    private bool HasWaypoint => (bool)UnsafeNetIdWaypoint;

    private Vector3 UnsafeWaypointWorldPosition => UnsafeNetIdWaypoint._pos;

    private Vector3 UnsafeWaypointLocalPosition
    {
        get
        {
            var parentPosition = WorldPosition - LocalPosition;
            return UnsafeWaypointWorldPosition - parentPosition;
        }
    }

    /// <inheritdoc />
    public UnityObjectWrapper<DoorBase> Base { get; }

    /// <inheritdoc />
    public DoorTypes DoorType => throw new NotImplementedException();

    /// <inheritdoc />
    public byte DoorId => Base.Instance.DoorId;

    /// <inheritdoc />
    public bool IsVisibleThrough => Base.Instance.IsVisibleThrough;

    /// <inheritdoc />
    public bool IsCollisionDisabled => CollisionDisabledReason == 0;

    /// <inheritdoc />
    public DoorBase.CollisionsDisablingReasons CollisionDisabledReason => Base.Instance._collidersStatus;

    /// <inheritdoc />
    public float OpenFraction => Base.Instance.GetExactState();

    /// <inheritdoc />
    public bool IsOpenComplete => OpenFraction is 1;

    /// <inheritdoc />
    public bool IsCloseComplete => OpenFraction is 0;

    /// <inheritdoc />
    public bool IsOpen
    {
        get => Base.Instance.IsConsideredOpen();
        set => Base.Instance.NetworkTargetState = value;
    }

    /// <inheritdoc />
    public bool IsBusy => Base.Instance.IsMoving;

    /// <inheritdoc />
    public bool IsLocked
    {
        get => Base.Instance.ActiveLocks > 0;
        set => SetLock(value);
    }

    /// <inheritdoc />
    public DoorLockReason LockReason
    {
        get => (DoorLockReason)Base.Instance.NetworkActiveLocks;
        set => Base.Instance.NetworkActiveLocks = (ushort)value;
    }

    /// <inheritdoc />
    public bool IsRoomsRegistered => Base.Instance.RoomsAlreadyRegistered;

    /// <inheritdoc />
    public IReadOnlyCollection<IRoom> Rooms { get; }

    /// <inheritdoc />
    public void SetLock(bool state, DoorLockReason reason = DoorLockReason.SpecialDoorFeature)
    {
        Base.Instance.ServerChangeLock(reason, state);
    }

    /// <inheritdoc />
    public override SpawnMessage GetSpawnMessage(NetworkConnection? conn = null)
    {
        return base.GetSpawnMessage(conn); // TODO: waypoint based spawn message
        var spawnMessage = base.GetSpawnMessage(conn);
        if (!HasWaypoint) return spawnMessage;
        spawnMessage.position = IsLevelGenerated ? UnsafeWaypointLocalPosition : UnsafeWaypointWorldPosition;
        return spawnMessage;
    }

    /// <inheritdoc />
    protected override void OnPositionChanged()
    {
        if (!HasWaypoint)
        {
            base.OnPositionChanged();
            return;
        }

        if (Vector3.Distance(WorldPosition, UnsafeWaypointWorldPosition) < 120.0F)
        {
            base.OnPositionChanged();
            return;
        }

        UnSpawn();
        Spawn();
        UnsafeNetIdWaypoint._pos = WorldPosition;
    }
}