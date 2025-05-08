using JetBrains.Annotations;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Structs;

namespace Qurre.API.World;

[PublicAPI]
public static class Map
{
    public static CassieList CassieList { get; internal set; } = new();

    public static AmbientSoundPlayer? AmbientSoundPlayer { get; internal set; }

    public static MapBroadcast Broadcast(string message, ushort duration, bool instant = false)
    {
        return new MapBroadcast(message, duration, instant, false);
    }

    public static MapBroadcast BroadcastAdmin(string message, ushort duration, bool instant = false)
    {
        return new MapBroadcast(message, duration, instant, true);
    }
}