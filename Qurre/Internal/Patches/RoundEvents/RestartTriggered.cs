using HarmonyLib;
using JetBrains.Annotations;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;
using RoundRestarting;

namespace Qurre.Internal.Patches.RoundEvents;

[HarmonyPatch(typeof(RoundRestart), nameof(RoundRestart.ChangeLevel))]
internal static class RestartTriggered
{
    [HarmonyPrefix, UsedImplicitly]
    private static bool Prefix()
    {
        var ev = new RoundRestartTriggeredEvent();
        ev.InvokeEvent();
        return true;
    }
}