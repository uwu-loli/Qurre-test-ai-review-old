using JetBrains.Annotations;
using MapGeneration;
using Qurre.API.Core;
using Qurre.API.Entities.Rooms.Lights;
using Qurre.API.Enums;

namespace Qurre.API.Entities.Rooms;

[PublicAPI]
public interface IGameRoom : IRoom
{
    UnityObjectWrapper<RoomIdentifier> Base { get; }

    RoomName Name { get; }

    RoomTypes RoomType { get; }

    new GameRoomLights Lights { get; }
}