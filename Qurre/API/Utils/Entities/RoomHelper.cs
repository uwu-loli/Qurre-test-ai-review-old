using System.Linq;
using MapGeneration;
using Qurre.API.Entities;
using Qurre.API.Entities.Rooms;
using Qurre.API.Enums;
using UnityEngine;

namespace Qurre.API.Utils.Entities;

public static class RoomHelper
{
    public static IRoom? GetByPoint(Vector3 worldPoint)
    {
        foreach (var room in EntityManager.GetAll<IGameRoom>())
        {
            if (room.EntireBounds.Contains(worldPoint))
                return room;

            if (room.WorldSubBounds.Any(bounds => bounds.Contains(worldPoint)))
                return room;
        }

        return null;
    }

    public static RoomTypes GetRoomType(RoomIdentifier roomIdentifier)
    {
        var roomName = roomIdentifier.gameObject.name;
        var transform = roomIdentifier.transform;

        var index = roomName.IndexOf('(');
        if (index > 0) roomName = roomName[..index].Trim();

        return roomName switch
        {
            "LCZ_ClassDSpawn" => RoomTypes.LczClassDSpawn,
            "LCZ_TCross" => RoomTypes.LczThreeWay,
            "LCZ_Crossing" => RoomTypes.LczCrossing,
            "LCZ_Curve" => RoomTypes.LczCurve,
            "LCZ_Straight" => RoomTypes.LczStraight,
            "LCZ_Airlock" => RoomTypes.LczAirlock,
            "LCZ_Armory" => RoomTypes.LczArmory,
            "LCZ_Cafe" => RoomTypes.LczCafe,
            "LCZ_Toilets" => RoomTypes.LczToilets,
            "LCZ_Plants" => RoomTypes.LczPlants,
            "LCZ_ChkpA" => RoomTypes.LczChkpA,
            "LCZ_ChkpB" => RoomTypes.LczChkpB,
            "LCZ_173" => RoomTypes.Lcz173,
            "LCZ_330" => RoomTypes.Lcz330,
            "LCZ_914" => RoomTypes.Lcz914,
            "LCZ_372" => RoomTypes.LczGr18,

            "HCZ_Crossing" => RoomTypes.HczCrossing,
            "HCZ_Curve" => RoomTypes.HczCurve,
            "HCZ_Straight" or "HCZ_Straight Variant" => RoomTypes.HczStraight,
            "HCZ_Intersection" => RoomTypes.HczIntersection,
            "HCZ_Corner_Deep" => RoomTypes.HczCornerDeep,
            "HCZ_Intersection_Junk" => RoomTypes.HczJunk,
            "HCZ_Straight_PipeRoom" => RoomTypes.HczPipe,
            "HCZ_Straight_C" => RoomTypes.HczToilets,
            "HCZ_Tesla_Rework" => RoomTypes.HczTesla,
            "HCZ_TArmory" => RoomTypes.HczArmory,
            "HCZ_MicroHID_New" => RoomTypes.HczHid,
            "HCZ_Nuke" => RoomTypes.HczNuke,
            "HCZ_ChkpA" => RoomTypes.HczCheckpointA,
            "HCZ_ChkpB" => RoomTypes.HczCheckpointB,
            "HCZ_049" => RoomTypes.Hcz049,
            "HCZ_079" => RoomTypes.Hcz079,
            "HCZ_096" => RoomTypes.Hcz096,
            "HCZ_106_Rework" => RoomTypes.Hcz106,
            "HCZ_939" => RoomTypes.Hcz939,
            "HCZ_Testroom" => RoomTypes.HczTest,
            "HCZ_Crossroom_Water" => RoomTypes.HczWater,

            "EZ_ThreeWay" => RoomTypes.EzThreeWay,
            "EZ_Crossing" => RoomTypes.EzCrossing,
            "EZ_Curve" => RoomTypes.EzCurve,
            "EZ_Straight" or "EZ_Cafeteria" or "EZ_StraightColumn" => RoomTypes.EzStraight,
            "EZ_upstairs" => RoomTypes.EzUpstairsPcs,
            "EZ_PCs_small" => RoomTypes.EzDownstairsPcs,
            "EZ_PCs" => RoomTypes.EzPcs,
            "EZ_Smallrooms1" => RoomTypes.EzSmall1,
            "EZ_Smallrooms2" => RoomTypes.EzSmall2,
            "EZ_Chef" => RoomTypes.EzChef,
            "EZ_Intercom" => RoomTypes.EzIntercom,
            "EZ_Shelter" => RoomTypes.EzShelter,
            "EZ_Endoof" => RoomTypes.EzVent,
            "EZ_GateA" => RoomTypes.EzGateA,
            "EZ_GateB" => RoomTypes.EzGateB,
            "HCZ_EZ_Checkpoint Part" => GetHczCheckpointRoomType(transform),
            "EZ_HCZ_Checkpoint Part" => GetEzCheckpointRoomType(transform),
            "EZ_CollapsedTunnel" => RoomTypes.EzCollapsedTunnel,

            "Outside" => RoomTypes.Surface,
            "PocketWorld" => RoomTypes.Pocket,

            _ => RoomTypes.Unknown
        };
    }

    private static RoomTypes GetHczCheckpointRoomType(Transform transform)
    {
        return transform.position.z > 75 ? RoomTypes.HczCheckpointB : RoomTypes.HczCheckpointA;
    }

    private static RoomTypes GetEzCheckpointRoomType(Transform transform)
    {
        return transform.position.z > 75 ? RoomTypes.EzCheckpointB : RoomTypes.EzCheckpointA;
    }
}