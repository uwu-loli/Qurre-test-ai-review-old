using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Entities.Rooms;
using Qurre.API.Exceptions;
using Scp079CameraBase = PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera;

namespace Qurre.API.Entities.Environment;

[PublicAPI]
public interface IScp079Camera : IReadOnlyTransformEntity
{
    UnityObjectWrapper<Scp079CameraBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsActive { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    IGameRoom Room { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsMain { get; }
}