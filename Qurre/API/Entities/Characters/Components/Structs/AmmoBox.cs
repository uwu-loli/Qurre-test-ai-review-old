using System.Collections.Generic;
using JetBrains.Annotations;
using Qurre.API.Enums;

namespace Qurre.API.Entities.Characters.Components.Structs;

[PublicAPI]
public sealed class AmmoBox
{
    private readonly Player _player;

    internal AmmoBox(Player player)
    {
        _player = player;
    }

    public ushort Ammo12Gauge
    {
        get => this[AmmoTypes.Ammo12Gauge];
        set => this[AmmoTypes.Ammo12Gauge] = value;
    }

    public ushort Ammo556
    {
        get => this[AmmoTypes.Ammo556];
        set => this[AmmoTypes.Ammo556] = value;
    }

    public ushort Ammo44Cal
    {
        get => this[AmmoTypes.Ammo44Cal];
        set => this[AmmoTypes.Ammo44Cal] = value;
    }

    public ushort Ammo762
    {
        get => this[AmmoTypes.Ammo762];
        set => this[AmmoTypes.Ammo762] = value;
    }

    public ushort Ammo9
    {
        get => this[AmmoTypes.Ammo9];
        set => this[AmmoTypes.Ammo9] = value;
    }

    public ushort this[AmmoTypes ammo]
    {
        get => _player.Inventory.Base.UserInventory.ReserveAmmo.GetValueOrDefault(ammo.GetItemType(), (ushort)0);
        set
        {
            _player.Inventory.Base.UserInventory.ReserveAmmo[ammo.GetItemType()] = value;
            _player.Inventory.Base.SendAmmoNextFrame = true;
        }
    }
}