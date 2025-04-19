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

[HarmonyPatch(typeof(DoorVariant), nameof(DoorVariant.ServerChangeLock))]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class LockDoor
{
    [HarmonyPrefix]
    private static bool Call(DoorVariant __instance, DoorLockReason reason, ref bool newState)
    {
        try
        {
            if (!EntityManager.TryGet(__instance, out IDoor? door)) return true;

            var ev = new LockDoorEvent(door, reason, newState);
            ev.InvokeEvent();

            newState = ev.NewState;
            return ev.IsAllowed;
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Map> {{Doors}} [LockDoor]: {e}\n{e.StackTrace}");
        }

        return true;
    }
}