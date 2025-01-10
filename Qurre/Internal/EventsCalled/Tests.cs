#if TESTS
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Mirror;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.Internal.EventsCalled;

[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
internal static class Tests
{
    [EventMethod(RoundEvents.Waiting), UsedImplicitly]
    internal static void Waiting()
    {
        #region Rooms

        Log.Info("----- ROOMS START -----");

        foreach (var room in Room.List)
        {
            if (room.Type != RoomType.Unknown)
                Log.Debug($"{room.Name}: {room.Type}");
            else
                Log.Warn($"{room.Name}: {room.Type}");
        }

        Log.Custom("----------------------------------------------");

        foreach (var roomType in Enum.GetValues(typeof(RoomType)))
        {
            if (Room.List.Any(x => $"{x.Type}" == $"{roomType}"))
            {
                Log.Debug($"Room \"{roomType}\" exist");
                continue;
            }

            if ($"{roomType}" == "Unknown") continue;
            Log.Warn($"Room \"{roomType}\" does not exist");
        }

        Log.Info("------ ROOMS END ------");

        #endregion

        #region Doors

        Log.Info("----- DOORS START -----");

        foreach (var door in Door.List)
            if (door.Type != DoorType.Unknown)
                Log.Debug(door.GetDebugString());
            else
                Log.Warn(door.GetDebugString());

        Log.Custom("----------------------------------------------");

        foreach (var doorType in Enum.GetValues(typeof(DoorType)))
        {
            if (Door.List.Any(x => $"{x.Type}" == $"{doorType}"))
            {
                Log.Debug($"Door \"{doorType}\" exist");
                continue;
            }

            if ($"{doorType}" == "Unknown") continue;
            Log.Warn($"Door \"{doorType}\" does not exist");
        }

        Log.Info("------ DOORS END ------");

        #endregion
    }

    [EventMethod(ServerEvents.GameConsoleCommand), UsedImplicitly]
    internal static void GetRoom(GameConsoleCommandEvent ev)
    {
        switch (ev.Name)
        {
            case "room_get":
            {
                var room = ev.Player.GamePlay.Room;

                ev.Allowed = false;
                ev.Reply = $"Name: {room.Name}; RoomName: {room.RoomName}; Type: {room.Type}";

                break;
            }

            case "room_tp":
            {
                ev.Allowed = false;

                var roomName = string.Join(' ', ev.Args);
                var room = Room.List.FirstOrDefault(x => x.Name == roomName);

                if (room is null)
                {
                    ev.Reply = $"Room \"{roomName}\" does not exist";
                    break;
                }

                ev.Player.MovementState.Position = room.WorldPosition + Vector3.up;

                break;
            }

            case "spawn_lift":
            {
                ev.Allowed = false;
                var prefabGameObject = NetworkClient.prefabs.First(x => x.Key == 2588580243).Value;
                var instanceGameObject = Object.Instantiate(prefabGameObject);
                instanceGameObject.transform.position = ev.Player.MovementState.Position;
                NetworkServer.Spawn(instanceGameObject);
                break;
            }
        }
    }
}
#endif