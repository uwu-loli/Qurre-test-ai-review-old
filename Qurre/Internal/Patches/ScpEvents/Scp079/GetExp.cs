using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using HarmonyLib;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.PlayableScps.Scp079.Rewards;
using Qurre.API;
using Qurre.API.Entities.Characters;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;
using RoleScp079 = Qurre.API.Entities.Characters.Components.Roles.Scp079;

namespace Qurre.Internal.Patches.ScpEvents.Scp079;

[HarmonyPatch(typeof(Scp079RewardManager), nameof(Scp079RewardManager.GrantExp))]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
internal static class GetExp
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Call(IEnumerable<CodeInstruction> _)
    {
        yield return new CodeInstruction(OpCodes.Ldarg_0); // instance [Scp079Role]
        yield return new CodeInstruction(OpCodes.Ldarg_1); // reward [int]
        yield return new CodeInstruction(OpCodes.Ldarg_2); // gainReason [Scp079HudTranslation]
        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetExp), nameof(Invoke)));
        yield return new CodeInstruction(OpCodes.Ret);
    }

    private static void Invoke(Scp079Role instance, int reward, Scp079HudTranslation gainReason)
    {
        try
        {
            if (!instance.TryGetOwner(out var referenceHub))
                return;

            var player = referenceHub.GetPlayer();
            if (player is null) return;

            var ev = new Scp079GetExpEvent(player, gainReason, reward);
            ev.InvokeEvent();

            if (!ev.IsAllowed) return;
            if (ev.Player.RoleInformation.CurrentRole is not RoleScp079 roleScp079) return;
            
            roleScp079.TierManager.Instance.ServerGrantExperience(ev.Amount, ev.Type);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <SCPs> {{Scp079}} [GetExp]: {e}\n{e.StackTrace}");
        }
    }
}