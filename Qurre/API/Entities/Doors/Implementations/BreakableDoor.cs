using Interactables.Interobjects.DoorUtils;
using Qurre.API.Core;
using Qurre.Internal.Attributes;
using BreakableDoorBase = Interactables.Interobjects.BreakableDoor;

namespace Qurre.API.Entities.Doors.Implementations;

[EntityWrapBindForFactory(typeof(BreakableDoorBase))]
internal sealed class BreakableDoor : Door, IBreakableDoor
{
    public BreakableDoor(BreakableDoorBase breakableDoorBase) : base(breakableDoorBase)
    {
        Base = breakableDoorBase;
    }

    /// <inheritdoc />
    public new UnityObjectWrapper<BreakableDoorBase> Base { get; }

    #region Game Object API

    /// <inheritdoc />
    public BrokenDoor BrokenPrefab => Base.Instance._brokenPrefab;

    #endregion

    /// <inheritdoc />
    public bool IsNonInteractable
    {
        get => Base.Instance._nonInteractable;
        set => Base.Instance._nonInteractable = value;
    }

    /// <inheritdoc />
    public DoorDamageType IgnoredDamageSources
    {
        get => Base.Instance.IgnoredDamageSources;
        set => Base.Instance.IgnoredDamageSources = value;
    }

    /// <inheritdoc />
    public bool IsBroken
    {
        get => Base.Instance.Network_destroyed;
        set
        {
            if (value) Base.Instance.Network_destroyed = value;
            else Repair();
        }
    }

    /// <inheritdoc />
    public float Health
    {
        get => Base.Instance.RemainingHealth;
        set => Base.Instance.RemainingHealth = value;
    }

    /// <inheritdoc />
    public float Health01
    {
        get => Base.Instance.GetHealthPercent();
        set => Health = value * MaxHealth;
    }

    /// <inheritdoc />
    public float MaxHealth
    {
        get => Base.Instance.MaxHealth;
        set => Base.Instance.MaxHealth = value;
    }

    /// <inheritdoc />
    public bool ServerDamage(float amount, DoorDamageType damageType = DoorDamageType.None)
    {
        return Base.Instance.ServerDamage(amount, damageType);
    }

    /// <inheritdoc />
    public void Repair()
    {
        Base.Instance.Network_destroyed = false;
        Health = MaxHealth;
    }
}