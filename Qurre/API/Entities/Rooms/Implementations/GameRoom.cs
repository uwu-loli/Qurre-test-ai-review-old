using System.Collections.Generic;
using System.Linq;
using MapGeneration;
using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.API.Entities.Characters;
using Qurre.API.Entities.Rooms.Lights;
using Qurre.API.Enums;
using Qurre.API.Utils;
using Qurre.API.Utils.Entities;
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
        RoomType = RoomHelper.GetRoomType(Base.Instance);
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
}