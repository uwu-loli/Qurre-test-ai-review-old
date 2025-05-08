using System;
using System.Linq;
using JetBrains.Annotations;
using MapGeneration;
using Qurre.API.Entities;
using Qurre.API.Entities.Rooms;
using UnityEngine;

namespace Qurre.API.World;

[PublicAPI]
public static class GlobalLights
{
    private static void ApplyToRooms(Action<IRoom> action, bool includeCustom = true, FacilityZone? zoneType = null)
    {
        var roomList = EntityManager.GetAll<IGameRoom>().Where(room => zoneType is null || room.Zone == zoneType)
            .Cast<IRoom>().ToList();

        // if include custom, then add range to list
        // TODO: include custom rooms
        //RoomManager.AllRooms.ForEach(action);

        foreach (var room in roomList)
        {
            action(room);
        }
    }

    public static void DisableLight(float? duration = null, bool includeCustom = true, FacilityZone? zoneType = null)
    {
        ApplyToRooms(room => room.Lights.Disable(duration), includeCustom, zoneType);
    }

    public static void ChangeColor(Color color, bool includeCustom = true, FacilityZone? zoneType = null)
    {
        ApplyToRooms(room => room.Lights.Color = color, includeCustom, zoneType);
    }

    public static void Intensity(float intensity, bool includeCustom = true, FacilityZone? zoneType = null)
    {
        ApplyToRooms(room => room.Lights.Intensity = intensity, includeCustom, zoneType);
    }

    public static void Reset(bool includeCustom = true, FacilityZone? zoneType = null)
    {
        ApplyToRooms(room => room.Lights.Reset(), includeCustom, zoneType);
    }
}