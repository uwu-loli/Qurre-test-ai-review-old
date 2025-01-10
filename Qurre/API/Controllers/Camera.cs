using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using Qurre.API.Controllers.Components;
using UnityEngine;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Camera : Entity<Scp079Camera, Camera>
{
    protected sealed override Scp079Camera UnsafeBase { get; }
    
    public Room Room { get; }

    public bool IsMain => Base.IsMain;

    public bool IsActive
    {
        get => Base.IsActive;
        set => Base.IsActive = value;
    }

    public float VerticalRotation
    {
        get => Base.VerticalRotation;
        set => Base.VerticalRotation = value;
    }

    public float HorizontalRotation
    {
        get => Base.HorizontalRotation;
        set => Base.HorizontalRotation = value;
    }

    public Vector2 VectorRotation
    {
        get => new(HorizontalRotation, VerticalRotation);
        set
        {
            HorizontalRotation = value.x;
            VerticalRotation = value.y;
        }
    }
    
    private Camera(Scp079Camera camera, Room? bindRoom)
    {
        UnsafeBase = camera;

        bindRoom ??= Room.Get(camera.Room);
        Room = bindRoom ?? throw new InvalidOperationException("Room is null");
        
        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    public static Camera? Get(Scp079Camera cameraBase)
    {
        return Get(cameraBase, room: null);
    }

    public static bool TryGet(Scp079Camera cameraBase, [NotNullWhen(true)] out Camera? camera)
    {
        return TryGet(cameraBase, room: null, out camera);
    }
    
    internal static Camera? Get(Scp079Camera cameraBase, Room? room)
    {
        if (!cameraBase) return null;
        return BaseToWrap.TryGetValue(cameraBase, out var camera) ? camera : new Camera(cameraBase, room);
    }

    internal static bool TryGet(Scp079Camera cameraBase, Room? room, [NotNullWhen(true)] out Camera? camera)
    {
        camera = Get(cameraBase, room);
        return camera is not null;
    }
}
