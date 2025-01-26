using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Interactables.Interobjects;
using JetBrains.Annotations;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Environment;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(ElevatorChamber), nameof(ElevatorChamber.Awake))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Lift
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(ElevatorChamber __instance)
    {
        try
        {
            _ = EntityManager.Get<ILift>(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Lift]: {e}\n{e.StackTrace}");
        }
    }
}