using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using InventorySystem.Items.Firearms.Attachments;
using Mirror;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Environment;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;

namespace Qurre.Internal.Patches.MapEvents.Objects;

[HarmonyPatch(typeof(WorkstationController), nameof(WorkstationController.Update))]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class WorkStationUpdate
{
    [HarmonyPrefix]
    private static bool Call(WorkstationController __instance)
    {
        if (__instance.Status == 0 || !EntityManager.TryGet(__instance, out IWorkStation? workStation))
            return false;

        var ev = new WorkStationUpdateEvent(workStation);
        ev.InvokeEvent();

        return ev.IsAllowed;
    }
}