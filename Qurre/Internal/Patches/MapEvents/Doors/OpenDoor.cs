using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Doors;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;

namespace Qurre.Internal.Patches.MapEvents.Doors;

[HarmonyPatch(typeof(DoorEventOpenerExtension), nameof(DoorEventOpenerExtension.Trigger))]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class OpenDoor
{
    [HarmonyPrefix]
    private static bool Call(DoorEventOpenerExtension __instance, DoorEventOpenerExtension.OpenerEventType eventType)
    {
        try
        {
            if (__instance == null || !EntityManager.TryGet(__instance.TargetDoor, out IDoor? door))
                return true;

            OpenDoorEvent ev = new(door, eventType);
            ev.InvokeEvent();

            return ev.IsAllowed;
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Map> {{Doors}} [OpenDoor]: {e}\n{e.StackTrace}");
        }

        return true;
    }
}