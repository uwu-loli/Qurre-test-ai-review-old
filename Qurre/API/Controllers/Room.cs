using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using MapGeneration;
using Mirror;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using Qurre.API.Controllers.Components;
using Qurre.API.Objects;
using UnityEngine;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Room : TransformableEntity<RoomIdentifier, Room>
{
    protected override RoomIdentifier UnsafeBase { get; }
    
    internal readonly Color DefaultColor;
    internal readonly RoomLightController[] GameLights;
    
    /// <inheritdoc />
    public override Vector3 WorldPosition
    {
        get => base.WorldPosition;
        set => throw new InvalidOperationException("That won't move it on the client and is guaranteed to break everything.");
    }

    /// <inheritdoc />
    public override Quaternion WorldRotation
    {
        get => base.WorldRotation;
        set => throw new InvalidOperationException("That won't rotate it on the client and is guaranteed to break everything.");
    }

    /// <inheritdoc />
    public override Vector3 WorldScale
    {
        get => base.WorldScale;
        set => throw new InvalidOperationException("That won't scale it on the client and is guaranteed to break everything.");
    }

    /// <inheritdoc />
    public override Vector3 LocalPosition
    {
        get => base.LocalPosition;
        set => throw new InvalidOperationException("That won't move it on the client and is guaranteed to break everything.");
    }

    /// <inheritdoc />
    public override Quaternion LocalRotation
    {
        get => base.LocalRotation;
        set => throw new InvalidOperationException("That won't rotate it on the client and is guaranteed to break everything.");
    }

    /// <inheritdoc />
    public override Vector3 LocalScale
    {
        get => base.LocalScale;
        set => throw new InvalidOperationException("That won't scale it on the client and is guaranteed to break everything.");
    }

    private Room(RoomIdentifier roomIdentifier)
    {
        UnsafeBase = roomIdentifier;
        RoomName = Base.Name;
        Shape = Base.Shape;
        
        GameLights = GameObject.GetComponentsInChildren<RoomLightController>();

        DefaultColor = GameLights.Length == 0 ? Color.white : GameLights[0].OverrideColor;

        foreach (var cameraBase in GameObject.GetComponentsInChildren<Scp079Camera>())
        {
            if (!Camera.TryGet(cameraBase, this, out var camera)) continue;
            Cameras.Add(camera);
        }

        Lights = new Lights(this);

        Type = GetType(Name, Transform);

        BaseToWrap[Base] = this;
    }

    public Lights Lights { get; private set; }

    public List<Door> Doors { get; } = [];
    public List<Camera> Cameras { get; } = [];

    public RoomName RoomName { get; }
    public RoomShape Shape { get; }
    public RoomType Type { get; }

    public Tesla? Tesla => GameObject.GetComponentInChildren<TeslaGate>()?.GetTesla();
    public string Name => GameObject.name;
    public IReadOnlyCollection<Player> Players => [..Player.List.Where(x => !x.IsHost && x.GamePlay.Room == this)];
    public bool LightsDisabled => GameLights.Length > 0 && GameLights.Any(x => !x.NetworkLightsEnabled);

    public FacilityZone Zone => Base.Zone;

    public void LightsOff(float duration)
    {
        foreach (RoomLightController light in GameLights)
            light.ServerFlickerLights(duration);
    }

    private static RoomType GetType(string name, Transform trans)
    {
        string rawName = name;
        int arr = rawName.IndexOf('(');
        if (arr > 0) rawName = rawName.Remove(arr, rawName.Length - arr).Trim();

#if TESTS
        Log.Custom($"rawName: {rawName};", "TESTS: API.Room");
        if (rawName.EndsWith(" Part"))
            Log.Custom($"ZZZ POSITION::::: {Addons.BetterColors.Red(trans.position.z)}");
#endif

        return rawName switch
        {
            "LCZ_ClassDSpawn" => RoomType.LczClassDSpawn,
            "LCZ_TCross" => RoomType.LczThreeWay,
            "LCZ_Crossing" => RoomType.LczCrossing,
            "LCZ_Curve" => RoomType.LczCurve,
            "LCZ_Straight" => RoomType.LczStraight,
            "LCZ_Airlock" => RoomType.LczAirlock,
            "LCZ_Armory" => RoomType.LczArmory,
            "LCZ_Cafe" => RoomType.LczCafe,
            "LCZ_Toilets" => RoomType.LczToilets,
            "LCZ_Plants" => RoomType.LczPlants,
            "LCZ_ChkpA" => RoomType.LczChkpA,
            "LCZ_ChkpB" => RoomType.LczChkpB,
            "LCZ_173" => RoomType.Lcz173,
            "LCZ_330" => RoomType.Lcz330,
            "LCZ_914" => RoomType.Lcz914,
            "LCZ_372" => RoomType.LczGr18,

            "HCZ_Crossing" => RoomType.HczCrossing,
            "HCZ_Curve" => RoomType.HczCurve,
            "HCZ_Straight" or "HCZ_Straight Variant" => RoomType.HczStraight,
            "HCZ_Intersection" => RoomType.HczIntersection,
            "HCZ_Corner_Deep" => RoomType.HczCornerDeep,
            "HCZ_Intersection_Junk" => RoomType.HczJunk,
            "HCZ_Straight_PipeRoom" => RoomType.HczPipe,
            "HCZ_Straight_C" => RoomType.HczToilets,
            "HCZ_Tesla_Rework" => RoomType.HczTesla,
            "HCZ_TArmory" => RoomType.HczArmory,
            "HCZ_MicroHID_New" => RoomType.HczHid,
            "HCZ_Nuke" => RoomType.HczNuke,
            "HCZ_ChkpA" => RoomType.HczChkpA,
            "HCZ_ChkpB" => RoomType.HczChkpB,
            "HCZ_049" => RoomType.Hcz049,
            "HCZ_079" => RoomType.Hcz079,
            "HCZ_096" => RoomType.Hcz096,
            "HCZ_106_Rework" => RoomType.Hcz106,
            "HCZ_939" => RoomType.Hcz939,
            "HCZ_Testroom" => RoomType.HczTest,
            "HCZ_Crossroom_Water" => RoomType.HczWater,

            "EZ_ThreeWay" => RoomType.EzThreeWay,
            "EZ_Crossing" => RoomType.EzCrossing,
            "EZ_Curve" => RoomType.EzCurve,
            "EZ_Straight" or
                "EZ_Cafeteria" or
                "EZ_StraightColumn" => RoomType.EzStraight,
            "EZ_upstairs" => RoomType.EzUpstairsPcs,
            "EZ_PCs_small" => RoomType.EzDownstairsPcs,
            "EZ_PCs" => RoomType.EzPcs,
            "EZ_Smallrooms1" => RoomType.EzSmall1,
            "EZ_Smallrooms2" => RoomType.EzSmall2,
            "EZ_Chef" => RoomType.EzChef,
            "EZ_Intercom" => RoomType.EzIntercom,
            "EZ_Shelter" => RoomType.EzShelter,
            "EZ_Endoof" => RoomType.EzVent,
            "EZ_GateA" => RoomType.EzGateA,
            "EZ_GateB" => RoomType.EzGateB,
            "HCZ_EZ_Checkpoint Part" => trans.position.z switch
            {
                // 60...90
                > 75 => RoomType.HczChkpA,
                _ => RoomType.HczChkpB
            },
            "EZ_HCZ_Checkpoint Part" => trans.position.z switch
            {
                // 60...90
                > 75 => RoomType.EzChkpA,
                _ => RoomType.EzChkpB
            },
            "EZ_CollapsedTunnel" => RoomType.EzCollapsedTunnel,

            "Outside" => RoomType.Surface,
            "PocketWorld" => RoomType.Pocket,

            _ => RoomType.Unknown
        };
    }

    public static Room? Get(RoomIdentifier roomIdentifier)
    {
        if (!roomIdentifier) return null;
        return BaseToWrap.TryGetValue(roomIdentifier, out var room) ? room : new Room(roomIdentifier);
    }

    public static bool TryGet(RoomIdentifier roomIdentifier, [NotNullWhen(true)] out Room? room)
    {
        room = Get(roomIdentifier);
        return room is not null;
    }
}