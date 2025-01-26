using JetBrains.Annotations;
using Qurre.API.Core;
using UnityEngine;

namespace Qurre.API.Entities.Rooms;

[PublicAPI]
public interface IGameRoomLight : IRoomLight, IEntity
{
    UnityObjectWrapper<RoomLightController> Base { get; }

    Color DefaultColor { get; set; }
}