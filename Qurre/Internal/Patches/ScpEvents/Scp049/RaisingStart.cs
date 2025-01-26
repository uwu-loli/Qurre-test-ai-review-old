using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using HarmonyLib;
using PlayerRoles.PlayableScps.Scp049;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Entities.Characters;
using Qurre.API.World.Entities.Player;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;

namespace Qurre.Internal.Patches.ScpEvents.Scp049;

using static Scp049ResurrectAbility;

[HarmonyPatch(typeof(Scp049ResurrectAbility), nameof(Scp049ResurrectAbility.ServerValidateAny))]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
internal static class RaisingStart
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Call(IEnumerable<CodeInstruction> _)
    {
        yield return new CodeInstruction(OpCodes.Ldarg_0); // instance [Scp049ResurrectAbility]
        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RaisingStart), nameof(Invoke)));
        yield return new CodeInstruction(OpCodes.Ret);
    }

    private static bool Invoke(Scp049ResurrectAbility instance)
    {
        if (instance.CurRagdoll == null)
            return false;

        var issuer = instance.Owner.GetPlayer();
        var target = instance.CurRagdoll.Info.OwnerHub.GetPlayer();

        if (target is null || issuer is null)
            return false;

        var ev = new Scp049RaisingStartEvent(issuer, target, instance.CurRagdoll)
        {
            IsAllowed = instance.IsCloseEnough(instance.CastRole.FpcModule.Position, instance._ragdollTransform.position) &&
                        IsSpawnableSpectator(target.ReferenceHub) &&
                        instance.CheckMaxResurrections(target.ReferenceHub) == ResurrectError.None &&
                        !instance.AnyConflicts(instance.CurRagdoll)
        };
        ev.InvokeEvent();
        return ev.IsAllowed;
    }
}