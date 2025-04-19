using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using MapGeneration.Distributors;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Structures;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(Scp079Generator), nameof(Scp079Generator.Start))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Generator
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(Scp079Generator __instance)
    {
        try
        {
            _ = EntityManager.Get<IGenerator>(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Generator]: {e}\n{e.StackTrace}");
        }
    }
}