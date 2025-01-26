using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Entities.Rooms.Lights;

namespace Qurre.API.Entities.Rooms;

[PublicAPI]
public interface ICustomRoom : ITransformEntity, IRoom
{
    new CustomRoomLights Lights { get; }
}