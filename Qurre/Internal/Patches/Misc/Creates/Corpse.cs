using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using PlayerRoles.Ragdolls;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Characters;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(RagdollManager), nameof(RagdollManager.ServerSpawnRagdoll))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Corpse
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(ReferenceHub owner, BasicRagdoll __result)
    {
        try
        {
            _ = EntityManager.Get<ICorpse>(__result);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Corpse]: {e}\n{e.StackTrace}");
        }
    }
}