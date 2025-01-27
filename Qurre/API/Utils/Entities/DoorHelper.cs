using System.Linq;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using JetBrains.Annotations;
using MapGeneration;
using Qurre.API.Enums;

namespace Qurre.API.Utils.Entities;

[PublicAPI]
public static class DoorHelper
{
    public static DoorTypes GetDoorType(DoorVariant doorVariant)
    {
        // by NameTag
        if (doorVariant.TryGetComponent(out DoorNametagExtension nameTagExtension))
        {
            var nameTag = nameTagExtension.GetName;
            
            if (!string.IsNullOrEmpty(nameTag))
            {
                return nameTag switch
                {
                    // LCZ
                    "LCZ_ARMORY" => DoorTypes.LczArmory,
                    "LCZ_CAFE" => DoorTypes.LczCafe,
                    "LCZ_WC" => DoorTypes.LczWc,
                    "GR18_INNER" => DoorTypes.LczGr18,
                    "GR18" => DoorTypes.LczGr18Gate,
                    "173_ARMORY" => DoorTypes.Lcz173Armory,
                    "173_GATE" => DoorTypes.Lcz173Gate,
                    "173_CONNECTOR" => DoorTypes.Lcz173Connector,
                    "173_BOTTOM" => DoorTypes.Lcz173Bottom,
                    "330" => DoorTypes.Lcz330,
                    "330_CHAMBER" => DoorTypes.Lcz330Chamber,
                    "914" => DoorTypes.Lcz914Gate,
                    "CHECKPOINT_LCZ_A" => DoorTypes.LczCheckpointA,
                    "CHECKPOINT_LCZ_B" => DoorTypes.LczCheckpointB,
                    // HCZ
                    "HCZ_ARMORY" => DoorTypes.HczArmory,
                    "HID_CHAMBER" => DoorTypes.HczHid,
                    "HID_LOWER" => DoorTypes.HczHidLower,
                    "HID_UPPER" => DoorTypes.HczHidUpper,
                    "049_ARMORY" => DoorTypes.Hcz049Armory,
                    "079_ARMORY" => DoorTypes.Hcz079Armory,
                    "079_FIRST" => DoorTypes.Hcz079First,
                    "079_SECOND" => DoorTypes.Hcz079Second,
                    "096" => DoorTypes.Hcz096,
                    "106_PRIMARY" => DoorTypes.Hcz106First,
                    "106_SECONDARY" => DoorTypes.Hcz106Second,
                    "939_CRYO" => DoorTypes.Hcz939,
                    // EZ
                    "INTERCOM" => DoorTypes.EzIntercom,
                    "GATE_A" => DoorTypes.EzGateA,
                    "GATE_B" => DoorTypes.EzGateB,
                    // Surface,
                    "SURFACE_GATE" => DoorTypes.SurfaceGate,
                    "SURFACE_NUKE" => DoorTypes.SurfaceNuke,
                    "ESCAPE_PRIMARY" => DoorTypes.SurfaceEscapeFirst,
                    "ESCAPE_SECONDARY" => DoorTypes.SurfaceEscapeSecond,
                    "ESCAPE_FINAL" => DoorTypes.SurfaceEscapeFinal,
                    "CHECKPOINT_EZ_HCZ_A" when doorVariant is CheckpointDoor checkpointDoor => GetEzCheckpointDoorType(checkpointDoor),

                    _ => DoorTypes.Unknown
                };
            }
        }

        // by Prefix
        var gameObjectName = doorVariant.gameObject.name;
        var namePrefix = gameObjectName.Split(' ')[0];
        
        return namePrefix switch
        {
            // Room-less
            "Prison" => DoorTypes.LczPrison,
            "914" => DoorTypes.Lcz914Chamber,
            "LCZ" => gameObjectName.StartsWith("LCZ PortallessBreakableDoor")
                ? DoorTypes.LczAirlock
                : DoorTypes.LczStandard,
            "HCZ" => gameObjectName.StartsWith("HCZ BulkDoor")
                ? DoorTypes.HczBulk
                : DoorTypes.HczStandard,
            "EZ" => DoorTypes.EzStandard,
            
            // Room-ness
            "Unsecured" => doorVariant.Rooms.Select(ri => ri.Name switch
                {
                    RoomName.Hcz049 => gameObjectName.Contains("(1)")
                        ? DoorTypes.Hcz173Gate
                        : DoorTypes.Hcz049Gate,
                    RoomName.HczCheckpointToEntranceZone => DoorTypes.EzCheckpointGate,
                    _ => DoorTypes.Unknown,
                })
                .First(),
            "Intercom" => doorVariant.Rooms.Any(ri =>
                ri.Name == RoomName.HczCheckpointToEntranceZone && ri.transform.position.z > 75)
                ? DoorTypes.EzCheckpointArmoryA
                : DoorTypes.EzCheckpointArmoryB,
            "Elevator" or "Nuke" => doorVariant.Rooms.Select(ri => ri.Name switch
                {
                    RoomName.LczCheckpointA => DoorTypes.ElevatorLczChkpA,
                    RoomName.LczCheckpointB => DoorTypes.ElevatorLczChkpB,

                    RoomName.HczCheckpointA => DoorTypes.ElevatorHczChkpA,
                    RoomName.HczCheckpointB => DoorTypes.ElevatorHczChkpB,

                    RoomName.Hcz049 => DoorTypes.Elevator049,

                    RoomName.HczWarhead => DoorTypes.ElevatorNuke,

                    RoomName.EzGateA => DoorTypes.ElevatorGateA,
                    RoomName.EzGateB => DoorTypes.ElevatorGateB,

                    RoomName.Outside => doorVariant is not ElevatorDoor elevatorDoor
                        ? DoorTypes.Unknown
                        : elevatorDoor.Group switch
                        {
                            ElevatorGroup.GateA => DoorTypes.EzGateA,
                            ElevatorGroup.GateB => DoorTypes.EzGateB,
                            _ => DoorTypes.Unknown
                        },

                    _ => DoorTypes.Unknown
                }).FirstOrDefault(dt => dt != DoorTypes.Unknown),

            _ => DoorTypes.Unknown
        };
    }

    private static DoorTypes GetEzCheckpointDoorType(CheckpointDoor checkpointDoor)
    {
        foreach (var roomIdentifier in checkpointDoor.Rooms)
        {
            var roomType = RoomHelper.GetRoomType(roomIdentifier);
            switch (roomType)
            {
                case RoomTypes.EzCheckpointA:
                    return DoorTypes.EzCheckpointA;
                case RoomTypes.EzCheckpointB:
                    return DoorTypes.EzCheckpointB;
            }
        }

        return DoorTypes.Unknown;
    }
}