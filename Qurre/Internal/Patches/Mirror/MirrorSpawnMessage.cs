using System;
using HarmonyLib;
using JetBrains.Annotations;
using Mirror;
using Qurre.API;
using Qurre.API.Core;
using Qurre.Internal.Misc;

namespace Qurre.Internal.Patches.Mirror;

[HarmonyPatch(typeof(NetworkServer), "SendSpawnMessage")]
internal static class MirrorSpawnMessage
{
    [HarmonyPrefix, UsedImplicitly]
    private static bool Prefix(NetworkIdentity identity, NetworkConnection conn)
    {
        if (identity.serverOnly)
            return false;

        try
        {
            if (conn is not NetworkConnectionToClient connectionToClient)
                return true;

            if (!identity.TryGetComponent(out EntityLink entityLink))
                return true;

            if (entityLink.Entity is not INetworkEntity networkEntity)
                return true;

            var spawnMessage = networkEntity.GetSpawnMessage(connectionToClient);
            conn.Send(spawnMessage);
            return false;
        }
        catch (Exception e)
        {
            Log.Error(e);
            return true;
        }
    }
}