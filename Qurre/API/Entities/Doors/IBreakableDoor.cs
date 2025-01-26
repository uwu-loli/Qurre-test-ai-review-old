using Interactables.Interobjects.DoorUtils;
using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using BreakableDoorBase = Interactables.Interobjects.BreakableDoor;

namespace Qurre.API.Entities.Doors;

[PublicAPI]
public interface IBreakableDoor : IDoor
{
    #region Properties

    new UnityObjectWrapper<BreakableDoorBase> Base { get; }

    #region Game Object API

    /// <exception cref="ObjectDestroyedException" />
    BrokenDoor BrokenPrefab { get; }

    #endregion

    /// <exception cref="ObjectDestroyedException" />
    bool IsNonInteractable { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    DoorDamageType IgnoredDamageSources { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    bool IsBroken { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float Health { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float Health01 { get; set; }

    /// <exception cref="ObjectDestroyedException" />
    float MaxHealth { get; set; }

    #endregion

    #region Methods

    /// <exception cref="ObjectDestroyedException" />
    bool ServerDamage(float amount, DoorDamageType damageType = DoorDamageType.None);

    /// <exception cref="ObjectDestroyedException" />
    void Repair();

    #endregion
}