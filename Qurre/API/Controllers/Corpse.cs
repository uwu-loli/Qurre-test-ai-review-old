using System;
using JetBrains.Annotations;
using Mirror;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using Qurre.API.World;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Corpse
{
    private Player _pl;

    internal Corpse(BasicRagdoll @base, Player? owner)
    {
        Base = @base;
        _pl = owner ?? Server.Host;
    }

    public Corpse(RoleTypeId type, Vector3 position, Quaternion rotation, DamageHandlerBase handler, Player owner)
        : this(type, position, rotation, handler, owner.UserInformation.Nickname)
    {
    }

    public Corpse(Vector3 position, Quaternion rotation, DamageHandlerBase handler, Player owner)
        : this(owner.RoleInformation.Role, position, rotation, handler, owner)
    {
    }

    public Corpse(RoleTypeId type, Vector3 position, Quaternion rotation, DamageHandlerBase handler, string nickname)
    {
        if (!PlayerRoleLoader.AllRoles.TryFind(out var role, x => x.Key == type))
            throw new Exception("Role not found: " + type);

        if (role.Value is not IRagdollRole dollyRole)
            throw new MissingComponentException("IRagdollRole component not found");

        GameObject gameObject = Object.Instantiate(dollyRole.Ragdoll.gameObject);

        if (!gameObject.TryGetComponent(out BasicRagdoll component))
        {
            Object.Destroy(gameObject);
            throw new MissingComponentException("BasicRagdoll component not found");
        }

        Base = component;
        _pl = Server.Host;

        Base.NetworkInfo = new RagdollData(Server.Host.ReferenceHub, handler,
            type, position, rotation, nickname, NetworkTime.time);

        NetworkServer.Spawn(component.gameObject);

        Map.Corpses.Add(this);
    }

    public BasicRagdoll Base { get; }
    public GameObject GameObject => Base.gameObject;

    public string Name
    {
        get => Base.Info.Nickname;
        set => Base.NetworkInfo = new RagdollData(Base.Info.OwnerHub, Base.Info.Handler,
            Role, Position, Rotation, value, Base.Info.CreationTime, Base.Info.Serial);
    }

    public RoleTypeId Role
    {
        get => Base.Info.RoleType;
        set => Base.NetworkInfo = new RagdollData(Base.Info.OwnerHub, Base.Info.Handler,
            value, Position, Rotation, Name, Base.Info.CreationTime, Base.Info.Serial);
    }

    public Vector3 Position
    {
        get
        {
            try
            {
                return Base.transform.position;
            }
            catch
            {
                return Vector3.zero;
            }
        }
        set
        {
            NetworkServer.UnSpawn(GameObject);
            Base.transform.position = value;
            NetworkServer.Spawn(GameObject);

            Base.NetworkInfo = new RagdollData(Base.Info.OwnerHub, Base.Info.Handler,
                Role, value, Rotation, Name, Base.Info.CreationTime, Base.Info.Serial);
        }
    }

    public Quaternion Rotation
    {
        get => Base.transform.localRotation;
        set
        {
            NetworkServer.UnSpawn(GameObject);
            Base.transform.localRotation = value;
            NetworkServer.Spawn(GameObject);

            Base.NetworkInfo = new RagdollData(Base.Info.OwnerHub, Base.Info.Handler,
                Role, Position, value, Name, Base.Info.CreationTime, Base.Info.Serial);
        }
    }

    public Vector3 Scale
    {
        get => Base.transform.localScale;
        set
        {
            NetworkServer.UnSpawn(GameObject);
            Base.transform.localScale = value;
            NetworkServer.Spawn(GameObject);
        }
    }

    public Player Owner
    {
        get => _pl;
        set
        {
            _pl = value;
            RagdollData info = Base.Info;
            Base.NetworkInfo =
                new RagdollData(value.ReferenceHub, info.Handler, info.StartPosition, info.StartRotation);
        }
    }

    public void Destroy()
    {
        NetworkServer.Destroy(GameObject);
        Map.Corpses.Remove(this);
    }
}