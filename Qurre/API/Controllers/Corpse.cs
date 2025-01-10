using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Mirror;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using Qurre.API.Controllers.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Corpse : NetworkEntity<BasicRagdoll, Corpse>
{
    protected sealed override BasicRagdoll UnsafeBase { get; }
    private Player _owner;

    public string CharacterName
    {
        get => Base.Info.Nickname;
        set => Base.NetworkInfo = new RagdollData(Base.Info.OwnerHub, Base.Info.Handler, 
            Role, WorldPosition, WorldRotation, value, Base.Info.CreationTime, Base.Info.Serial);
    }

    public RoleTypeId Role
    {
        get => Base.Info.RoleType;
        set => Base.NetworkInfo = new RagdollData(Base.Info.OwnerHub, Base.Info.Handler,
            value, WorldPosition, WorldRotation, CharacterName, Base.Info.CreationTime, Base.Info.Serial);
    }

    public Player Owner
    {
        get => _owner;
        set
        {
            _owner = value;
            var info = Base.Info;
            Base.NetworkInfo =
                new RagdollData(value.ReferenceHub, info.Handler, info.StartPosition, info.StartRotation);
        }
    }

    private Corpse(BasicRagdoll ragdollBase, Player? owner)
    {
        UnsafeBase = ragdollBase;
        _owner = owner ?? Server.Host;

        BaseToWrap[Base] = this;
        AddEntityLink();
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

        var gameObject = Object.Instantiate(dollyRole.Ragdoll.gameObject);

        if (!gameObject.TryGetComponent(out BasicRagdoll component))
        {
            Object.Destroy(gameObject);
            throw new MissingComponentException("BasicRagdoll component not found");
        }

        UnsafeBase = component;
        _owner = Server.Host;

        Base.NetworkInfo = new RagdollData(Server.Host.ReferenceHub, handler,
            type, position, rotation, nickname, NetworkTime.time);

        NetworkServer.Spawn(component.gameObject);
        
        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    private void UpdateNetworkInfo()
    {
        Base.NetworkInfo = new RagdollData(
            Base.Info.OwnerHub,
            Base.Info.Handler,
            Role,
            WorldPosition,
            WorldRotation,
            CharacterName,
            Base.Info.CreationTime,
            Base.Info.Serial);
    }

    protected override void OnPositionUpdated() => UpdateNetworkInfo();
    protected override void OnRotationUpdated() => UpdateNetworkInfo();

    internal static Corpse? Get(BasicRagdoll ragdollBase, Player? owner)
    {
        if (!ragdollBase) return null;
        return BaseToWrap.TryGetValue(ragdollBase, out var corpse) ? corpse : new Corpse(ragdollBase, owner);
    }

    public static Corpse? Get(BasicRagdoll ragdollBase)
    {
        if (!ragdollBase) return null;
        Player? player = null;
        if (ragdollBase.NetworkInfo.OwnerHub)
            player = Player.Get(ragdollBase.NetworkInfo.OwnerHub);
        return Get(ragdollBase, player);
    }

    public static bool TryGet(BasicRagdoll ragdollBase, [NotNullWhen(true)] out Corpse? corpse)
    {
        corpse = Get(ragdollBase);
        return corpse is not null;
    }
}