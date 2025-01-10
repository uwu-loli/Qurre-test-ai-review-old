using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using JetBrains.Annotations;
using MapGeneration;
using Qurre.API.Controllers.Components;
using Qurre.API.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Door : GeneratedNetworkEntity<DoorVariant, Door>
{
    protected sealed override DoorVariant UnsafeBase { get; }
    private List<Room> _rooms = [];

    private Door(DoorVariant doorBase)
    {
        UnsafeBase = doorBase;

        RefreshRooms();
        Type = SetupDoorType();
        
#if TESTS
        WriteDebugLog();
#endif

        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    public Door(Vector3 position, DoorPrefabs prefab, Quaternion? rotation = null, DoorPermissions? permissions = null)
    {
        PrefabType = prefab;

        UnsafeBase = Object.Instantiate(prefab.GetPrefab());

        Transform.position = position;
        Transform.rotation = rotation ?? new Quaternion();
        Base.RequiredPermissions = permissions ?? new DoorPermissions();

        Spawn();

        RefreshRooms();
        Type = SetupDoorType();
        
#if TESTS
        WriteDebugLog();
#endif
        
        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    public DoorPrefabs PrefabType { get; init; }

    public DoorType Type { get; }
    public bool IsLift => Base is ElevatorDoor;
    public bool IsBreakable => Base is BreakableDoor;

    public IEnumerable<Room> Rooms => RoomsSet;
    private HashSet<Room> RoomsSet { get; } = [];
    
    public bool IsPryable
    {
        get
        {
            if (Base is not PryableDoor pry)
                return false;

            return ReferenceHub.TryGetHostHub(out var hub) && pry.TryPryGate(hub);
        }
    }

    public DoorPermissions RequiredPermissions
    {
        get => Base.RequiredPermissions;
        set => Base.RequiredPermissions = value;
    }

    public bool IsOpened
    {
        get => Base.IsConsideredOpen();
        set => Base.NetworkTargetState = value;
    }

    public bool IsLocked
    {
        get => Base.ActiveLocks > 0;
        set => Base.ServerChangeLock(DoorLockReason.SpecialDoorFeature, value);
    }

    public bool IsBroken
    {
        get
        {
            if (Base is BreakableDoor damageableDoor)
                return damageableDoor.IsDestroyed;
            return false;
        }
    }

    public bool Break()
    {
        if (Base is not BreakableDoor damageableDoor)
            return false;

        damageableDoor.IsDestroyed = true;
        return true;
    }

    public static Door? Get(DoorVariant doorBase)
    {
        if (!doorBase) return null;
        return BaseToWrap.TryGetValue(doorBase, out var door) ? door : new Door(doorBase);
    }

    public static bool TryGet(DoorVariant doorBase, [NotNullWhen(true)] out Door? door)
    {
        door = Get(doorBase);
        return door is not null;
    }

    private void RefreshRooms()
    {
        RoomsSet.Clear();
        if (Base.Rooms is null) return;
        foreach (var roomIdentifier in Base.Rooms)
        {
            if (!Room.TryGet(roomIdentifier, out var room)) continue;
            RoomsSet.Add(room);
        }
    }

#if TESTS
    internal string GetDebugString()
    {
        var nameTagExtension = GameObject.GetComponent<DoorNametagExtension>();
        var nameTagExists = (bool)nameTagExtension;
        var nameTag = nameTagExists ? $"[YES] {nameTagExtension.name}" : $"[NO] {GameObject.name}";
        return $"Door [ NameTag: {nameTag} | Type: {Type} ];";
    }
    
    private void WriteDebugLog()
    {
        var deubString = GetDebugString();
        Log.Custom(deubString, "TESTS: API.Door");
    }
#endif

    private DoorType SetupDoorType()
    {
        // by NameTag
        if (Base.TryGetComponent(out DoorNametagExtension nameTagExtension))
        {
            return nameTagExtension.GetName switch
            {
                // LCZ
                "LCZ_ARMORY" => DoorType.LczArmory,
                "LCZ_CAFE" => DoorType.LczCafe,
                "LCZ_WC" => DoorType.LczWc,
                "GR18_INNER" => DoorType.LczGr18,
                "GR18" => DoorType.LczGr18Gate,
                "173_ARMORY" => DoorType.Lcz173Armory,
                "173_GATE" => DoorType.Lcz173Gate,
                "173_CONNECTOR" => DoorType.Lcz173Connector,
                "173_BOTTOM" => DoorType.Lcz173Bottom,
                "330" => DoorType.Lcz330,
                "330_CHAMBER" => DoorType.Lcz330Chamber,
                "914" => DoorType.Lcz914Gate,
                "CHECKPOINT_LCZ_A" => DoorType.LczCheckpointA,
                "CHECKPOINT_LCZ_B" => DoorType.LczCheckpointB,
                // HCZ
                "HCZ_ARMORY" => DoorType.HczArmory,
                "HID_CHAMBER" => DoorType.HczHid,
                "HID_LOWER" => DoorType.HczHidLower,
                "HID_UPPER" => DoorType.HczHidUpper,
                "049_ARMORY" => DoorType.Hcz049Armory,
                "079_ARMORY" => DoorType.Hcz079Armory,
                "079_FIRST" => DoorType.Hcz079First,
                "079_SECOND" => DoorType.Hcz079Second,
                "096" => DoorType.Hcz096,
                "106_PRIMARY" => DoorType.Hcz106First,
                "106_SECONDARY" => DoorType.Hcz106Second,
                "939_CRYO" => DoorType.Hcz939,
                // EZ
                "INTERCOM" => DoorType.EzIntercom,
                "GATE_A" => DoorType.EzGateA,
                "GATE_B" => DoorType.EzGateB,
                // Surface,
                "SURFACE_GATE" => DoorType.SurfaceGate,
                "SURFACE_NUKE" => DoorType.SurfaceNuke,
                "ESCAPE_PRIMARY" => DoorType.SurfaceEscapeFirst,
                "ESCAPE_SECONDARY" => DoorType.SurfaceEscapeSecond,
                "ESCAPE_FINAL" => DoorType.SurfaceEscapeFinal,
                "CHECKPOINT_EZ_HCZ_A" => Base.Rooms.Any(ri =>
                    ri.Name == RoomName.HczCheckpointToEntranceZone && ri.transform.position.z > 75) 
                    ? DoorType.EzCheckpointA
                    : DoorType.EzCheckpointB,

                _ => DoorType.Unknown
            };
        }

        var name = GameObject.name;
        var namePrefix = name.Split(' ')[0];

        // by Prefix
        return namePrefix switch
        {
            // Roomless
            "Prison" => DoorType.LczPrison,
            "914" => DoorType.Lcz914Chamber,
            "LCZ" => name.StartsWith("LCZ PortallessBreakableDoor")
                ? DoorType.LczAirlock
                : DoorType.LczStandard,
            "HCZ" => name.StartsWith("HCZ BulkDoor")
                ? DoorType.HczBulk
                : DoorType.HczStandard,
            "EZ" => DoorType.EzStandard,
            
            // Roomness
            "Unsecured" => Base.Rooms.Select(ri => ri.Name switch
                {
                    RoomName.Hcz049 => name.Contains("(1)")
                        ? DoorType.Hcz173Gate
                        : DoorType.Hcz049Gate,
                    RoomName.HczCheckpointToEntranceZone => DoorType.EzCheckpointGate,
                    _ => DoorType.Unknown,
                })
                .First(),
            "Intercom" => Base.Rooms.Any(ri =>
                ri.Name == RoomName.HczCheckpointToEntranceZone && ri.transform.position.z > 75)
                ? DoorType.EzCheckpointArmoryA
                : DoorType.EzCheckpointArmoryB,
            "Elevator" or "Nuke" => Base.Rooms.Select(ri => ri.Name switch
                {
                    RoomName.LczCheckpointA => DoorType.ElevatorLczChkpA,
                    RoomName.LczCheckpointB => DoorType.ElevatorLczChkpB,

                    RoomName.HczCheckpointA => DoorType.ElevatorHczChkpA,
                    RoomName.HczCheckpointB => DoorType.ElevatorHczChkpB,

                    RoomName.Hcz049 => DoorType.Elevator049,

                    RoomName.HczWarhead => DoorType.ElevatorNuke,

                    RoomName.EzGateA => DoorType.ElevatorGateA,
                    RoomName.EzGateB => DoorType.ElevatorGateB,

                    RoomName.Outside => Base is not ElevatorDoor elevatorDoor
                        ? DoorType.Unknown
                        : elevatorDoor.Group switch
                        {
                            ElevatorGroup.GateA => DoorType.EzGateA,
                            ElevatorGroup.GateB => DoorType.EzGateB,
                            _ => DoorType.Unknown
                        },

                    _ => DoorType.Unknown
                }).FirstOrDefault(dt => dt != DoorType.Unknown),

            _ => DoorType.Unknown
        };
    }
}