using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using MapGeneration.Distributors;
using Qurre.API;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(Locker), nameof(Locker.Start))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Locker1
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(Locker __instance)
    {
        try
        {
            _ = API.Controllers.Locker.Get(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Locker]: {e}\n{e.StackTrace}");
        }
    }
}