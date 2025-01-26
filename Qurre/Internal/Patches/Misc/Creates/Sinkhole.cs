using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Hazards;
using JetBrains.Annotations;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Hazards;

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
            _ = EntityManager.Get<ISinkhole>(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Sinkhole]: {e}\n{e.StackTrace}");
        }
    }
}