using System;
using System.Diagnostics.CodeAnalysis;
using AdminToys;
using HarmonyLib;
using JetBrains.Annotations;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.AdminToys;

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
            _ = EntityManager.Get<ILightPoint>(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [LightPoint]: {e}\n{e.StackTrace}");
        }
    }
}