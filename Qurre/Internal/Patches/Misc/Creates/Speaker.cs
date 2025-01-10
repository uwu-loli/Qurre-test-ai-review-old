using System;
using System.Diagnostics.CodeAnalysis;
using AdminToys;
using HarmonyLib;
using JetBrains.Annotations;
using Mirror;
using Qurre.API;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(NetworkBehaviour), nameof(NetworkBehaviour.OnStartServer))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Speaker
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(NetworkBehaviour __instance)
    {
        if (__instance is not SpeakerToy speakerToy) return;
        
        try
        {
            _ = API.Controllers.Speaker.Get(speakerToy);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Speaker]: {e}\n{e.StackTrace}");
        }
    }
}