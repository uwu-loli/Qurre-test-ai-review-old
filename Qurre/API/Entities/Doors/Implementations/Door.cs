using System.Collections.Generic;
using System.Linq;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using Qurre.API.Addons;
using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.API.Entities.Rooms;
using Qurre.API.Enums;
using Qurre.API.Exceptions;
using Qurre.API.Utils.Entities;
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
        NetIdWaypoint = GameObject.Instance.GetComponent<NetIdWaypoint>();

        if (Base.Instance.Rooms is null)
            Base.Instance.RegisterRooms();

        Rooms = Base.Instance.Rooms is null
            ? []
            : Base.Instance.Rooms.Select(EntityManager.GetOrException<IGameRoom>).ToList();

        DoorType = DoorHelper.GetDoorType(Base.Instance);

        PrefabType = NetworkIdentity.Instance.assetId switch
        {
            Prefabs.AssetIdDoorLCZ => DoorPrefabs.DoorLCZ,
            Prefabs.AssetIdDoorHCZ => DoorPrefabs.DoorHCZ,
            Prefabs.AssetIdDoorEZ => DoorPrefabs.DoorEZ,
            Prefabs.AssetIdDoorBulkHCZ => DoorPrefabs.BulkHCZ,
            _ => DoorPrefabs.Unknown
        };
    }

    private UnityObjectWrapper<NetIdWaypoint> NetIdWaypoint { get; }

    /// <exception cref="ObjectDestroyedException" />
    private Vector3 WaypointWorldPosition => NetIdWaypoint.Instance._pos;

    /// <exception cref="ObjectDestroyedException" />
    private Vector3 WaypointLocalPosition
    {
        get
        {
            var parentPosition = WorldPosition - LocalPosition;
            return WaypointWorldPosition - parentPosition;
        }
    }

    /// <inheritdoc />
    public DoorPrefabs PrefabType { get; }

    /// <inheritdoc />
    public UnityObjectWrapper<DoorBase> Base { get; }

    /// <inheritdoc />
    public DoorTypes DoorType { get; private set; }

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
    public IReadOnlyList<IGameRoom> Rooms { get; }

    /// <inheritdoc />
    public DoorPermissions RequiredPermissions
    {
        get => Base.Instance.RequiredPermissions;
        set => Base.Instance.RequiredPermissions = value;
    }

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
        if (!NetIdWaypoint.IsAlive) return spawnMessage;
        spawnMessage.position = IsLevelGenerated ? WaypointLocalPosition : WaypointWorldPosition;
        return spawnMessage;
    }

    /// <inheritdoc />
    protected override void OnPositionChanged()
    {
        if (!NetIdWaypoint.IsAlive)
        {
            base.OnPositionChanged();
            return;
        }

        if (Vector3.Distance(WorldPosition, WaypointWorldPosition) < 120.0F)
        {
            base.OnPositionChanged();
            return;
        }

        UnSpawn();
        Spawn();
        NetIdWaypoint.Instance._pos = WorldPosition;
    }
}