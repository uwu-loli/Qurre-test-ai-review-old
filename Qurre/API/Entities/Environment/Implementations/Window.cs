using Footprinting;
using PlayerRoles;
using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.Internal.Attributes;
using WindowBase = BreakableWindow;

namespace Qurre.API.Entities.Environment.Implementations;

[EntityWrapBindForFactory(typeof(WindowBase))]
internal sealed class Window(WindowBase windowBase) : LevelEntity(windowBase.gameObject), IWindow
{
    /// <inheritdoc />
    public UnityObjectWrapper<WindowBase> Base { get; } = windowBase;

    /// <inheritdoc />
    public bool IsBroken
    {
        get => Base.Instance.NetworkisBroken;
        set => Base.Instance.NetworkisBroken = value;
    }

    /// <inheritdoc />
    public Footprint LastAttacker => Base.Instance.LastAttacker;

    /// <inheritdoc />
    public float Health
    {
        get => Base.Instance.health;
        set
        {
            Base.Instance.health = value;
            if (Base.Instance.health > 0) return;
            Base.Instance.isBroken = true;
        }
    }

    /// <inheritdoc />
    public bool PreventScpDamage
    {
        get => Base.Instance._preventScpDamage;
        set => Base.Instance._preventScpDamage = value;
    }

    /// <inheritdoc />
    public bool CheckCanDamage(RoleTypeId roleType)
    {
        return Base.Instance.CheckDamagePerms(roleType);
    }

    /// <inheritdoc />
    public void Damage(float amount)
    {
        Base.Instance.ServerDamageWindow(amount);
    }
}