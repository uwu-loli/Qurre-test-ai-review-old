using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using InventorySystem.Items.Firearms.Attachments;
using JetBrains.Annotations;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Environment;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(WorkstationController), nameof(WorkstationController.Start))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class WorkStation
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(WorkstationController __instance)
    {
        try
        {
            _ = EntityManager.Get<IWorkStation>(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Camera]: {e}\n{e.StackTrace}");
        }
    }
}
