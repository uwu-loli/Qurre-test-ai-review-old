using System;
using HarmonyLib;
using InventorySystem;
using InventorySystem.Items;
using JetBrains.Annotations;
using Qurre.API;
using Qurre.API.Entities;
using Qurre.API.Entities.Items;

// ReSharper disable once InconsistentNaming

namespace Qurre.Internal.Patches.Misc.Creates;

[HarmonyPatch(typeof(Inventory), nameof(Inventory.CreateItemInstance))]
internal static class Item
{
    [HarmonyPostfix, UsedImplicitly]
    private static void Call(ref ItemBase __result)
    {
        try
        {
            _ = EntityManager.Get<IItem>(__result);
        }
        catch (Exception e)
        {
            Log.Error($"Patch Error - <Misc> {{Creates}} [Item]: {e}\n{e.StackTrace}");
        }
    }
}