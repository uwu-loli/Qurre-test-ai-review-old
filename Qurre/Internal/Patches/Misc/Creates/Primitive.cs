using System;
using System.Diagnostics.CodeAnalysis;
using AdminToys;
using HarmonyLib;
using JetBrains.Annotations;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.AdminToys;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(PrimitiveObjectToy), nameof(PrimitiveObjectToy.Start))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Primitive
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(PrimitiveObjectToy __instance)
    {
        try
        {
            _ = EntityManager.Get<IPrimitive>(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Primitive]: {e}\n{e.StackTrace}");
        }
    }
}