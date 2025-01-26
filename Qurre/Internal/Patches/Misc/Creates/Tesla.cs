using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Environment;
using UnityEngine;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(TeslaGate), nameof(TeslaGate.Start))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Tesla
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(TeslaGate __instance)
    {
        try
        {
            _ = EntityManager.Get<ITesla>(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Tesla]: {e}\n{e.StackTrace}");
        }
    }
}
