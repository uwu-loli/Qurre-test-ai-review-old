using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using PlayerRoles.Ragdolls;
using Qurre.API;

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
            _ = API.Controllers.Corpse.Get(__result, owner.GetPlayer());
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Corpse]: {e}\n{e.StackTrace}");
        }
    }
}