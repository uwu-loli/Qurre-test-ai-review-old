#if TESTS
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Mirror;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Entities;
using Qurre.API.Entities.Doors;
using Qurre.API.Entities.Rooms;
using Qurre.API.Enums;
using Qurre.Events;
using Qurre.Events.Structs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.Internal.EventsCalled;

internal static class Tests
{
    [EventMethod(RoundEvents.Waiting), UsedImplicitly]
    internal static void Waiting()
    {
        #region Rooms

        var rooms = EntityManager.GetAll<IGameRoom>();

        Log.Info("----- ROOMS START -----");

        foreach (var room in rooms)
        {
            if (room.RoomType != RoomTypes.Unknown)
                Log.Debug($"{room.Name}: {room.RoomType}");
            else
                Log.Warn($"{room.Name}: {room.RoomType}");
        }

        Log.Custom("----------------------------------------------");

        foreach (var roomType in Enum.GetValues(typeof(RoomTypes)))
        {
            if (rooms.Any(x => $"{x.RoomType}" == $"{roomType}"))
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

        var doors = EntityManager.GetAll<IDoor>();

        Log.Info("----- DOORS START -----");

        foreach (var door in doors)
            if (door.DoorType != DoorTypes.Unknown)
                Log.Debug(door);
            else
                Log.Warn(door);

        Log.Custom("----------------------------------------------");

        foreach (var doorType in Enum.GetValues(typeof(DoorTypes)))
        {
            if (doors.Any(x => $"{x.DoorType}" == $"{doorType}"))
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

                if (room is IGameRoom gameRoom)
                {
                    ev.Allowed = false;
                    ev.Reply = $"Name: {gameRoom.Name}; Type: {gameRoom.RoomType}";
                }

                break;
            }

            case "room_tp":
            {
                ev.Allowed = false;

                var roomName = string.Join(' ', ev.Args);
                var room = EntityManager.GetAll<IGameRoom>().FirstOrDefault(gr => gr.Name.ToString() == roomName);

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
                var prefabGameObject = NetworkClient.prefabs.First(kvp => kvp.Key == 2588580243).Value;
                var instanceGameObject = Object.Instantiate(prefabGameObject);
                instanceGameObject.transform.position = ev.Player.MovementState.Position;
                NetworkServer.Spawn(instanceGameObject);
                break;
            }
        }
    }
}
#endif