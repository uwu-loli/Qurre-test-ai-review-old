using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using Qurre.API;

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(Scp079InteractableBase), nameof(Scp079InteractableBase.OnRegistered))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Camera
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(Scp079InteractableBase __instance)
    {
        if (__instance is not Scp079Camera cameraBase) return;
        
        try
        {
            _ = API.Controllers.Camera.Get(cameraBase);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Camera]: {e}\n{e.StackTrace}");
        }
    }
}
