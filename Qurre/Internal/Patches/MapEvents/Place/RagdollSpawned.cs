using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using PlayerRoles.Ragdolls;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Characters;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;

namespace Qurre.Internal.Patches.MapEvents.Place;

[HarmonyPatch(typeof(RagdollManager), nameof(RagdollManager.ServerSpawnRagdoll))]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class CorpseSpawned
{
    [HarmonyPostfix]
    private static void Call(BasicRagdoll __result)
    {
        try
        {
            if (__result == null || !EntityManager.TryGet(__result, out ICorpse? corpse))
                return;

            var ev = new CorpseSpawnedEvent(corpse);
            ev.InvokeEvent();
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Map> {{Place}} [CorpseSpawned]: {e}\n{e.StackTrace}");
        }
    }
}