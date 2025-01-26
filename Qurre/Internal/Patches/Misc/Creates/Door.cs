using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using JetBrains.Annotations;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Doors;
using Qurre.API.World.Entities;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(DoorVariant), nameof(DoorVariant.RegisterRooms))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Door
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(DoorVariant __instance)
    {
        try
        {
            _ = EntityManager.Get<IDoor>(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Door]: {e}\n{e.StackTrace}");
        }
    }
}
