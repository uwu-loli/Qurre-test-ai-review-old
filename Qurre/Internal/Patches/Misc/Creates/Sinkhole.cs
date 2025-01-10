using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Hazards;
using JetBrains.Annotations;
using Qurre.API;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(SinkholeEnvironmentalHazard), nameof(SinkholeEnvironmentalHazard.Start))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Sinkhole
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(SinkholeEnvironmentalHazard __instance)
    {
        try
        {
            _ = API.Controllers.Sinkhole.Get(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Sinkhole]: {e}\n{e.StackTrace}");
        }
    }
}