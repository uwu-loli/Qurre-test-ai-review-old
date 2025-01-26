using System;
using System.Diagnostics.CodeAnalysis;
using PlayerRoles;
using Qurre.API.Addons;
using Qurre.API.Attributes;
using Qurre.API.Entities.Characters.Components.Roles;
using Qurre.Events;
using Qurre.Events.Structs;

namespace Qurre.Internal.EventsCalled;

[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
internal static class Player
{
    [EventMethod(PlayerEvents.Join)]
    private static void Join(JoinEvent ev)
    {
        ServerConsole.AddLog($"Player {ev.Player.UserInformation.Nickname} ({ev.Player.UserInformation.UserId}) " +
                             $"({ev.Player.UserInformation.Id}) connected. iP: {ev.Player.UserInformation.Ip}",
            ConsoleColor.Magenta);
    }

    [EventMethod(PlayerEvents.Leave)]
    private static void LeaveClears(LeaveEvent ev)
    {
        if (Scp173.IgnoredPlayers.Contains(ev.Player))
            Scp173.IgnoredPlayers.Remove(ev.Player);
    }

    [EventMethod(PlayerEvents.Spawn)]
    private static void BlockSpawnTeleport(SpawnEvent ev)
    {
        if (!ev.Player.GamePlay.BlockSpawnTeleport)
            return;

        ev.Position = ev.Player.MovementState.Position;
    }

    [EventMethod(PlayerEvents.Spawn)]
    private static void SetMaxHp(SpawnEvent ev)
    {
        if (ev.Player.ReferenceHub.roleManager.CurrentRole is IHealthbarRole healthRole)
            ev.Player.HealthInformation.MaxHp = healthRole.MaxHealth;
        else
            ev.Player.HealthInformation.MaxHp = 0;

        ev.Player.HealthInformation.Hp = ev.Player.HealthInformation.MaxHp;
    }

    [EventMethod(PlayerEvents.Spawn)]
    private static void UpdateRole(SpawnEvent ev)
    {
        if (ev.Role is not RoleTypeId.Spectator and not RoleTypeId.None and not RoleTypeId.Overwatch)
            ev.Player.RoleInformation.CachedRole = ev.Role;

        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        ev.Player.RoleInformation.SetupRole();
    }
}