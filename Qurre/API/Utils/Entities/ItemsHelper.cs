using System.Linq;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using Qurre.API.Entities;
using Qurre.API.Entities.Items;

namespace Qurre.API.Utils.Entities;

public static class ItemsHelper
{
    public static IItem? GetItemByBase(ItemBase itemBase)
    {
        return EntityManager.Get<IItem>(itemBase);
    }

    public static IItem? GetItemBySerial(ushort serial)
    {
        return EntityManager.GetAll<IItem>().FirstOrDefault(item => item.Serial == serial);
    }

    public static IPickup? GetPickupByBase(ItemPickupBase pickupBase)
    {
        return EntityManager.Get<IPickup>(pickupBase);
    }
}