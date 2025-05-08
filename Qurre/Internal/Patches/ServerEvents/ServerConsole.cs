using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Qurre.API;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;
using Console = GameCore.Console;

namespace Qurre.Internal.Patches.ServerEvents;

[HarmonyPatch(typeof(Console), nameof(Console.TypeCommand))]
internal static class ServerConsole
{
    [HarmonyPrefix]
    [UsedImplicitly]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static bool Call(string cmd, ref string __result)
    {
        try
        {
            var arr = cmd.Split(' ');
            var commandName = arr[0].ToLower();
            var commandArgs = arr.Skip(1).ToArray();

            var ev = new ServerConsoleCommandEvent(cmd, commandName, commandArgs);
            ev.InvokeEvent();

            __result = ev.Reply;
            return ev.Allowed;
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Server> [ServerConsole]: {e}\n{e.StackTrace}");
            return true;
        }
    }
}