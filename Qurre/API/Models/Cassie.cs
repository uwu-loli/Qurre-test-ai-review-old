using JetBrains.Annotations;
using Qurre.API.World;
using Respawning;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Cassie(string message, bool makeHold, bool makeNoise)
{
    public static bool IsLocked { get; set; }

    public string Message { get; set; } = message;
    public bool Hold { get; set; } = makeHold;
    public bool Noise { get; set; } = makeNoise;
    public bool IsActive { get; private set; }

    public void Send()
    {
        if (IsActive) return;
        IsActive = true;
        RespawnEffectsController.PlayCassieAnnouncement(Message, Hold, Noise);
    }

    public static void Send(string msg, bool makeHold = false, bool makeNoise = false, bool instant = false)
    {
        Map.CassieList.Add(new Cassie(msg, makeHold, makeNoise), instant);
    }

    internal static void ForceEnd()
    {
        if (Map.CassieList.Count > 0)
            Map.CassieList[0].Send();
    }
}