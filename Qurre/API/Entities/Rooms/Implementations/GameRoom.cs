using System.Collections.Generic;
using System.Linq;
using MapGeneration;
using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.API.Entities.Characters;
using Qurre.API.Entities.Rooms.Lights;
using Qurre.API.Enums;
using Qurre.API.Utils;
using Qurre.Internal.Attributes;
using UnityEngine;

namespace Qurre.API.Entities.Rooms.Implementations;

[EntityWrapBindForFactory(typeof(RoomIdentifier))]
internal sealed class GameRoom : ReadOnlyTransformEntity, IGameRoom
{
    public GameRoom(RoomIdentifier roomBase) : base(roomBase.gameObject)
    {
        Base = roomBase;
        Base.Instance.TryAssignId();

        WorldSubBounds = LocalSubBounds
            .Select(bounds => BoundsHelper.CalculateWorldBoundsByRelative(bounds, GameObject.Instance.transform))
            .ToArray();

        EntireBounds = BoundsHelper.CalculateRoomEntireBounds(Base.Instance);

        Lights = new GameRoomLights(this);
        RoomType = GetRoomType();
    }

    /// <inheritdoc />
    public Bounds[] LocalSubBounds => Base.Instance.SubBounds;

    /// <inheritdoc />
    public Bounds[] WorldSubBounds { get; }

    /// <inheritdoc />
    public Bounds EntireBounds { get; }

    /// <inheritdoc />
    public UnityObjectWrapper<RoomIdentifier> Base { get; }

    /// <inheritdoc />
    public RoomName Name => Base.Instance.Name;

    /// <inheritdoc />
    public FacilityZone Zone => Base.Instance.Zone;

    /// <inheritdoc />
    public RoomTypes RoomType { get; }

    /// <inheritdoc />
    public GameRoomLights Lights { get; }

    /// <inheritdoc />
    IRoomLights IRoom.Lights => Lights;

    /// <inheritdoc />
    public IEnumerable<Player> GetPlayersInside()
    {
        return Player.List.Where(player => player.GamePlay.Room == this);
    }

    private RoomTypes GetRoomType()
    {
        var roomName = GameObject.Instance.name;
        var transform = GameObject.Instance.transform;

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
            "HCZ_ChkpA" => RoomTypes.HczChkpA,
            "HCZ_ChkpB" => RoomTypes.HczChkpB,
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
            "EZ_Straight" or
                "EZ_Cafeteria" or
                "EZ_StraightColumn" => RoomTypes.EzStraight,
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
            "HCZ_EZ_Checkpoint Part" => transform.position.z switch
            {
                // 60...90
                > 75 => RoomTypes.HczChkpA,
                _ => RoomTypes.HczChkpB
            },
            "EZ_HCZ_Checkpoint Part" => transform.position.z switch
            {
                // 60...90
                > 75 => RoomTypes.EzChkpA,
                _ => RoomTypes.EzChkpB
            },
            "EZ_CollapsedTunnel" => RoomTypes.EzCollapsedTunnel,

            "Outside" => RoomTypes.Surface,
            "PocketWorld" => RoomTypes.Pocket,

            _ => RoomTypes.Unknown
        };
    }
}