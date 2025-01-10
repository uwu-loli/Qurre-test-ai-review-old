using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using Qurre.API;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(BreakableWindow), nameof(BreakableWindow.Awake))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Window
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(BreakableWindow __instance)
    {
        try
        {
            _ = API.Controllers.Window.Get(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Camera]: {e}\n{e.StackTrace}");
        }
    }
}
