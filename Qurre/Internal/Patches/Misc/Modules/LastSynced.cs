using HarmonyLib;
using JetBrains.Annotations;
using Mirror;
using PlayerRoles.FirstPersonControl.NetworkMessages;
using Qurre.API;
using Qurre.API.Entities.Characters;
using UnityEngine;

namespace Qurre.Internal.Patches.Misc.Modules;

[HarmonyPatch(typeof(FpcFromClientMessage), nameof(FpcFromClientMessage.ProcessMessage))]
internal static class LastSynced
{
    [HarmonyPrefix]
    [UsedImplicitly]
    private static void Call(NetworkConnection sender)
    {
        if (!ReferenceHub.TryGetHubNetID(sender.identity.netId, out ReferenceHub? hub))
            return;

        Player? player = hub.GetPlayer();

        if (player is null)
            return;

        player.LastSynced = Time.time;
    }
}