using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using Mirror;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.AdminToys;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(NetworkBehaviour), nameof(NetworkBehaviour.OnStartServer))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class ShootingTarget
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(NetworkBehaviour __instance)
    {
        if (__instance is not AdminToys.ShootingTarget shootingTarget) return;

        try
        {
            _ = EntityManager.Get<IShootingTarget>(shootingTarget);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [ShootingTarget]: {e}\n{e.StackTrace}");
        }
    }
}
