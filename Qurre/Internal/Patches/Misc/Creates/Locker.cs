using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Structures;
using LockerBase = MapGeneration.Distributors.Locker;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(LockerBase), nameof(LockerBase.Start))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Locker
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(LockerBase __instance)
    {
        try
        {
            _ = EntityManager.Get<ILocker>(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Locker]: {e}\n{e.StackTrace}");
        }
    }
}