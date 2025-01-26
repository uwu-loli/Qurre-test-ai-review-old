using System.Linq;
using Qurre.API.Entities;
using Qurre.API.Entities.Rooms;
using UnityEngine;

namespace Qurre.API.Utils.Entities;

public static class RoomHelper
{
    public static IRoom? GetByPoint(Vector3 worldPoint)
    {
        foreach (var room in EntityManager.GetAll<IGameRoom>())
        {
            if (room.EntireBounds.Contains(worldPoint))
                return room;

            if (room.WorldSubBounds.Any(bounds => bounds.Contains(worldPoint)))
                return room;
        }

        return null;
    }
}