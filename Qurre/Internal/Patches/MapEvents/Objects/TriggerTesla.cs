using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Mirror;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Characters;
using Qurre.API.Entities.Environment;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;
using UnityEngine;

namespace Qurre.Internal.Patches.MapEvents.Objects;

[HarmonyPatch(typeof(TeslaGateController), nameof(TeslaGateController.FixedUpdate))]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
internal static class TriggerTesla
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Call(IEnumerable<CodeInstruction> _)
    {
        yield return new CodeInstruction(OpCodes.Ldarg_0); // instance [TeslaGateController]
        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(TriggerTesla), nameof(Invoke)));
        yield return new CodeInstruction(OpCodes.Ret);
    }

    private static void Invoke(TeslaGateController instance)
    {
        try
        {
            if (instance == null)
                return;

            if (!NetworkServer.active)
            {
                foreach (var teslaGate2 in TeslaGate.AllGates)
                    teslaGate2.ClientSideCode();
                return;
            }

            List<Player> players = [.. Player.List.Where(x => !x.IsHost && x.RoleInformation.IsAlive)];

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var teslaGate in TeslaGate.AllGates)
            {
                if (!teslaGate.isActiveAndEnabled)
                    continue;

                if (teslaGate.InactiveTime > 0f)
                {
                    teslaGate.NetworkInactiveTime = Mathf.Max(0f, teslaGate.InactiveTime - Time.fixedDeltaTime);
                    continue;
                }

                if (!EntityManager.TryGet(teslaGate, out ITesla? tesla) || !tesla.IsEnabled)
                    continue;

                bool idling = false;
                bool activated = false;

                // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                foreach (var player in players)
                {
                    var inIdleRange = teslaGate.IsInIdleRange(player.ReferenceHub);
                    if (!inIdleRange) continue;

                    var inRageRange = teslaGate.PlayerInRange(player.ReferenceHub);

                    TriggerTeslaEvent ev = new(player, tesla, inIdleRange, inRageRange);
                    ev.InvokeEvent();

                    if (!ev.IsAllowed)
                        continue;

                    idling = true;

                    if (!activated && ev.InRageRange && !teslaGate.InProgress)
                        activated = true;
                }

                if (activated)
                    teslaGate.ServerSideCode();
                if (idling != teslaGate.isIdling)
                    teslaGate.ServerSideIdle(activated);
            }
        }
        catch (NullReferenceException)
        {
            //Debug.Log(e);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Map> {{Objects}} [TriggerTesla]: {e}\n{e.StackTrace}");
        }
    }
}