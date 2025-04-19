using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Doors;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;

namespace Qurre.Internal.Patches.MapEvents.Doors;

[HarmonyPatch(typeof(BreakableDoor), nameof(BreakableDoor.ServerDamage))]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class DamageDoor
{
    [HarmonyPrefix]
    private static bool Call(BreakableDoor __instance, ref float hp, DoorDamageType type)
    {
        try
        {
            if (!EntityManager.TryGet(__instance, out IBreakableDoor? breakableDoor)) return true;

            var ev = new DamageDoorEvent(breakableDoor, type, hp);
            ev.InvokeEvent();

            hp = ev.Damage;
            return ev.IsAllowed;
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Map> {{Doors}} [DamageDoor]: {e}\n{e.StackTrace}");
        }

        return true;
    }
}