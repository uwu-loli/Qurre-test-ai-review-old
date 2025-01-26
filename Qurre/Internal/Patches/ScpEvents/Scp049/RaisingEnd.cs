using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using PlayerRoles.PlayableScps.Scp049;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Entities.Characters;
using Qurre.API.World.Entities.Player;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;

namespace Qurre.Internal.Patches.ScpEvents.Scp049;

[HarmonyPatch(typeof(Scp049ResurrectAbility), nameof(Scp049ResurrectAbility.ServerComplete))]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class RaisingEnd
{
    [HarmonyPrefix]
    private static bool Call(Scp049ResurrectAbility __instance)
    {
        if (__instance.CurRagdoll == null)
            return false;

        var issuer = __instance.Owner.GetPlayer();
        var target = __instance.CurRagdoll.Info.OwnerHub.GetPlayer();

        if (target is null || issuer is null)
            return false;

        var ev = new Scp049RaisingEndEvent(issuer, target, __instance.CurRagdoll);
        ev.InvokeEvent();
        return ev.IsAllowed;
    }
}