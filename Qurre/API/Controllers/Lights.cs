using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Lights
{
    internal Lights(Room room)
    {
        Room = room;
    }

    public Room Room { get; }

    public bool LockChange { get; set; }

    public bool Override
    {
        get => Room.GameLights.Any(x => x.NetworkOverrideColor != Room.DefaultColor);
        set
        {
            if (LockChange)
            {
                Log.Debug(
                    $"Lights locked. Called field.set: [Override]. Called from {Assembly.GetCallingAssembly().GetName().Name}");
                return;
            }

            if (value)
                Log.Debug(
                    $"Override.set = true is not supported in Default Room. Called from {Assembly.GetCallingAssembly().GetName().Name}");
            else
                foreach (var light in Room.GameLights)
                    light.NetworkOverrideColor = Room.DefaultColor;
        }
    }

    public float Intensity
    {
        get => 1;
        set
        {
            if (LockChange)
            {
                Log.Debug(
                    $"Lights locked. Called field.set: [Intensity]. Called from {Assembly.GetCallingAssembly().GetName().Name}");
                return;
            }

            Log.Debug($"Lights Intensity.set doesnt support on default rooms. Called from {Assembly.GetCallingAssembly().GetName().Name}");
        }
    }

    public Color Color
    {
        get => Room.GameLights.Length > 0 ? Room.GameLights[0].NetworkOverrideColor : Room.DefaultColor;
        set
        {
            if (LockChange)
            {
                Log.Debug(
                    $"Lights locked. Called field.set: [Color]. Called from {Assembly.GetCallingAssembly().GetName().Name}");
                return;
            }

            foreach (var light in Room.GameLights)
                light.NetworkOverrideColor = value;
        }
    }
}