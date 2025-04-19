using System;
using System.Reflection;
using JetBrains.Annotations;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace Qurre.API.Entities.Characters.Components;

[PublicAPI]
public sealed class MovementState
{
    private readonly Player _player;

    internal MovementState(Player player)
    {
        _player = player;
    }

    public Transform CameraReference => _player.ReferenceHub.PlayerCameraReference;
    public Transform Transform => _player.ReferenceHub.transform;

    public Vector3 Velocity => _player.ReferenceHub.GetVelocity();

    public Vector3 Position
    {
        get => _player.GameObject.transform.position;
        set => _player.ReferenceHub.TryOverridePosition(value);
    }

    public Vector3 Rotation
    {
        get => _player.GameObject.transform.eulerAngles;
        set => _player.ReferenceHub.TryOverrideRotation(value);
    }

    public Vector3 Scale
    {
        get => _player.ReferenceHub.transform.localScale;
        set
        {
            try
            {
                if (_player.Disconnected)
                {
                    Log.Debug(
                        $"Scale: Player already disconnected. Called from: {Assembly.GetCallingAssembly().GetName().Name}");
                    return;
                }

                _player.ReferenceHub.transform.localScale = value;
                foreach (var player in Player.List)
                    Network.SendSpawnMessage?.Invoke(null, [_player.ClassManager.netIdentity, player.Connection]);
            }
            catch (Exception ex)
            {
                Log.Error($"Scale error: {ex}");
            }
        }
    }

    public Vector3 Gravity
    {
        get => _player.ReferenceHub.roleManager.CurrentRole is IFpcRole role
            ? role.FpcModule.Motor.GravityController.Gravity
            : Vector3.zero;
        set
        {
            if (_player.ReferenceHub.roleManager.CurrentRole is IFpcRole role)
                role.FpcModule.Motor.GravityController.Gravity = value;
        }
    }
}