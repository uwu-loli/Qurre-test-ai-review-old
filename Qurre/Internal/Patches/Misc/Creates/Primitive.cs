using System;
using System.Diagnostics.CodeAnalysis;
using AdminToys;
using HarmonyLib;
using JetBrains.Annotations;
using Qurre.API;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(PrimitiveObjectToy), nameof(PrimitiveObjectToy.Start))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class Primitive
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(PrimitiveObjectToy __instance)
    {
        try
        {
            _ = API.Controllers.Primitive.Get(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Primitive]: {e}\n{e.StackTrace}");
        }
    }
}