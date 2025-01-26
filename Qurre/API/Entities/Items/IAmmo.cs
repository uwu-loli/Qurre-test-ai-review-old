using InventorySystem.Items.Firearms.Ammo;
using JetBrains.Annotations;
using Qurre.API.Core;

namespace Qurre.API.Entities.Items;

[PublicAPI]
public interface IAmmo : IItem
{
    new UnityObjectWrapper<AmmoItem> Base { get; }

    int UnitPrice { get; set; }
}