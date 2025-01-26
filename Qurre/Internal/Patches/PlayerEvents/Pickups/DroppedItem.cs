using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using InventorySystem;
using InventorySystem.Items.Pickups;
using JetBrains.Annotations;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Entities;
using Qurre.API.Entities.Characters;
using Qurre.API.Entities.Items;
using Qurre.API.Entities.Items.Implementations;
using Qurre.API.World.Entities.Player;
using Qurre.Events.Structs;
using Qurre.Internal.EventsManager;

// ReSharper disable once InconsistentNaming

namespace Qurre.Internal.Patches.PlayerEvents.Pickups;

[HarmonyPatch(typeof(InventoryExtensions), nameof(InventoryExtensions.ServerDropItem))]
internal static class DroppedItem
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(Inventory inv, ItemPickupBase __result)
    {
        if (!EntityManager.TryGet(__result, out IPickup? pickup))
            return;

        if (!Player.TryGet(inv._hub, out var player))
            return;
        
        new DroppedItemEvent(player, pickup).InvokeEvent();
    }
}