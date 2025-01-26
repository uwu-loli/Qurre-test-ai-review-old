using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.API.Entities.Rooms;
using Qurre.Internal.Attributes;
using Scp079CameraBase = PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera;

namespace Qurre.API.Entities.Environment.Implementations;

[EntityWrapBindForFactory(typeof(Scp079CameraBase))]
internal sealed class Scp079Camera : ReadOnlyTransformEntity, IScp079Camera
{
    public Scp079Camera(Scp079CameraBase cameraBase) : base(cameraBase.gameObject)
    {
        Base = cameraBase;
        Room = EntityManager.GetOrException<IGameRoom>(Base.Instance.Room);
    }

    public UnityObjectWrapper<Scp079CameraBase> Base { get; }

    public bool IsActive
    {
        get => Base.Instance.IsActive;
        set => Base.Instance.IsActive = value;
    }

    public IGameRoom Room { get; }

    public bool IsMain => Base.Instance.IsMain;
}