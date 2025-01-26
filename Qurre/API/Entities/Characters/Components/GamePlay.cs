using InventorySystem.Disarming;
using JetBrains.Annotations;
using MapGeneration;
using Qurre.API.Entities.Environment;
using Qurre.API.Utils.Entities;
using UnityEngine;
using IRoom = Qurre.API.Entities.Rooms.IRoom;

namespace Qurre.API.Entities.Characters.Components;

[PublicAPI]
public sealed class GamePlay
{
    private readonly Player _player;

    internal GamePlay(Player player)
    {
        _player = player;
        BlockSpawnTeleport = false;
    }

    public bool BlockSpawnTeleport { get; set; }

    public bool IsCuffed => _player.ReferenceHub.inventory.IsDisarmed();

    public FacilityZone CurrentZone => Room?.Zone ?? FacilityZone.None;

    public bool InOverwatch
    {
        get => _player.ReferenceHub.serverRoles.IsInOverwatch;
        set => _player.ReferenceHub.serverRoles.IsInOverwatch = value;
    }

    public bool GodMode
    {
        get => _player.ClassManager.GodMode;
        set => _player.ClassManager.GodMode = value;
    }

    public IRoom? Room
    {
        get => RoomHelper.GetByPoint(_player.MovementState.Position);
        set
        {
            if (value is null) return;
            _player.MovementState.Position = value.WorldPosition + Vector3.up * 2;
        }
    }

    public ILift? Lift
    {
        get => _player.MovementState.Position.GetLift();
        set
        {
            if (value is null) return;
            _player.MovementState.Position = value.WorldPosition + Vector3.up * 2;
        }
    }

    public Player? Cuffer
    {
        get
        {
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var disarmed in DisarmedPlayers.Entries)
                if (disarmed.DisarmedPlayer == _player.ReferenceHub.netId)
                    return disarmed.Disarmer.GetPlayer();

            return null;
        }
        set
        {
            for (var i = 0; i < DisarmedPlayers.Entries.Count; i++)
                if (DisarmedPlayers.Entries[i].DisarmedPlayer == _player.Inventory.Base.netId)
                {
                    DisarmedPlayers.Entries.RemoveAt(i);
                    break;
                }

            if (value != null)
                _player.Inventory.Base.SetDisarmedStatus(value.Inventory.Base);
        }
    }
}