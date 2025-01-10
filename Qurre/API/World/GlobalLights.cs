using System.Linq;
using JetBrains.Annotations;
using MapGeneration;
using Qurre.API.Controllers;
using UnityEngine;

namespace Qurre.API.World;

[PublicAPI]
public static class GlobalLights
{
    public static void TurnOff(float duration)
    {
        foreach (var room in Room.List)
            room.LightsOff(duration);
    }

    public static void TurnOff(float duration, FacilityZone zone)
    {
        foreach (var room in Room.List.Where(x => x.Zone == zone))
            room.LightsOff(duration);
    }

    public static void ChangeColor(Color color, bool customToo = true, bool lockChange = false, bool ignoreLock = false)
    {
        foreach (var room in Room.List)
        {
            if (ignoreLock) room.Lights.LockChange = false;
            room.Lights.Color = color;
            if (lockChange) room.Lights.LockChange = true;
        }
    }

    public static void ChangeColor(Color color, FacilityZone zone)
    {
        foreach (var room in Room.List.Where(x => x.Zone == zone))
            room.Lights.Color = color;
    }

    public static void Intensivity(float intensive, bool customToo = false)
    {
        foreach (var room in Room.List)
            room.Lights.Intensity = intensive;
    }

    public static void Intensivity(float intensive, FacilityZone zone)
    {
        foreach (var room in Room.List.Where(x => x.Zone == zone))
            room.Lights.Intensity = intensive;
    }

    public static void SetToDefault(bool customToo = true, bool ignoreLock = false)
    {
        foreach (var room in Room.List)
        {
            if (ignoreLock) room.Lights.LockChange = false;
            room.Lights.Override = false;
        }
    }
}