using JetBrains.Annotations;
using PlayerRoles;
using Qurre.API.Entities.Characters.Components.Roles;
using RemoteAdmin;

namespace Qurre.API.Entities.Characters.Components;

[PublicAPI]
public sealed class RoleInformation
{
    private readonly Player _player;

    internal RoleInformation(Player player)
    {
        _player = player;
        CachedRole = RoleTypeId.None;
    }

    public PlayerRoleBase Base => _player.ReferenceHub.roleManager.CurrentRole;
    public ServerRoles ServerRoles => _player.ReferenceHub.serverRoles;
    public QueryProcessor QueryProcessor => _player.ReferenceHub.queryProcessor;

    public bool IsAlive => Team != Team.Dead;
    public bool IsScp => Team == Team.SCPs;
    public bool IsHuman => IsAlive && !IsScp;
    public bool IsDirty => _player.ReferenceHub.IsDirty();

    public Role? CurrentRole { get; private set; }

    public RoleTypeId CachedRole { get; internal set; }

    public Team Team => _player.Disconnected ? CachedRole.GetTeam() : _player.ReferenceHub.GetTeam();

    public Faction Faction => _player.Disconnected ? CachedRole.GetFaction() : _player.ReferenceHub.GetFaction();

    public RoleTypeId RoleType
    {
        get => _player.Disconnected ? CachedRole : _player.ReferenceHub.GetRoleId();
        set => _player.ReferenceHub.roleManager.ServerSetRole(value, RoleChangeReason.RemoteAdmin);
    }

    public void SetNew(RoleTypeId newRole, RoleChangeReason reason)
    {
        _player.ReferenceHub.roleManager.ServerSetRole(newRole, reason);
    }

    public void SetNew(RoleTypeId newRole, RoleChangeReason reason, RoleSpawnFlags spawnFlags)
    {
        _player.ReferenceHub.roleManager.ServerSetRole(newRole, reason, spawnFlags);
    }

    public void SetSyncModel(RoleTypeId roleTypeId)
    {
        foreach (var referenceHub in ReferenceHub.AllHubs)
            _player.ReferenceHub.connectionToClient.Send(new RoleSyncInfo(_player.ReferenceHub, roleTypeId,
                referenceHub));
    }

    public void SetupRole()
    {
        CurrentRole = RoleFactory.Create(_player.ReferenceHub.roleManager.CurrentRole);
    }
}