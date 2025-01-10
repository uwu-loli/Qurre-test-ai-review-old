using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using InventorySystem;
using Qurre.API;
using Qurre.API.Addons.Items;
using Qurre.API.Controllers;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;

namespace Qurre.Internal.Patches.PlayerEvents.Items;

[HarmonyPatch(typeof(Inventory), nameof(Inventory.ServerSelectItem))]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class ChangeItem
{
    [HarmonyPrefix]
    private static bool Call(Inventory __instance, ushort itemSerial)
    {
        try
        {
            if (itemSerial == __instance.CurItem.SerialNumber)
                return false;

            var player = __instance._hub.GetPlayer();
            if (player is null) return false;
            
            if (!Item.TryGet(__instance.CurInstance, out var oldItem)) return true;
            var newItem = itemSerial == 0 ? null : Item.Get(itemSerial);

            ChangeItemEvent ev = new(player, oldItem, newItem);
            ev.InvokeEvent();

            return ev.Allowed;
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Player> {{Items}} [ChangeItem]: {e}\n{e.StackTrace}");
        }

        return true;
    }
}