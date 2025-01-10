using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using MapGeneration;
using Qurre.API;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(RoomIdentifier), nameof(RoomIdentifier.Awake))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Room
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(RoomIdentifier __instance)
    {
        try
        {
            _ = API.Controllers.Room.Get(__instance);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Room]: {e}\n{e.StackTrace}");
        }
    }
}