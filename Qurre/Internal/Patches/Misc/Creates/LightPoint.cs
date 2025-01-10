using System;
using System.Diagnostics.CodeAnalysis;
using AdminToys;
using HarmonyLib;
using JetBrains.Annotations;
using Qurre.API;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(LightSourceToy), nameof(LightSourceToy.Start))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class LightPoint
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(LightSourceToy __instance)
    {
        try
        {
            _ = API.Controllers.LightPoint.Get(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [LightPoint]: {e}\n{e.StackTrace}");
        }
    }
}