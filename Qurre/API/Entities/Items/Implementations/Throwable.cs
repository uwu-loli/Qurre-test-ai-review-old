using InventorySystem.Items.ThrowableProjectiles;
using PlayerRoles.FirstPersonControl;
using Qurre.API.Core;
using Qurre.Internal.Attributes;

namespace Qurre.API.Entities.Items.Implementations;

[EntityWrapBindForFactory(typeof(ThrowableItem))]
internal class Throwable(ThrowableItem itemBase) : Item(itemBase), IThrowable
{
    public new UnityObjectWrapper<ThrowableItem> Base { get; } = itemBase;

    public UnityObjectWrapper<ThrownProjectile> Projectile => Base.Instance.Projectile;

    public float PinPullTime
    {
        get => Base.Instance._pinPullTime;
        set => Base.Instance._pinPullTime = value;
    }

    public void Throw(bool fullForce = true)
    {
        var projectileSettings =
            fullForce ? Base.Instance.FullThrowSettings : Base.Instance.WeakThrowSettings;
        Base.Instance.ServerThrow(projectileSettings.StartVelocity, projectileSettings.UpwardsFactor,
            projectileSettings.StartTorque,
            ThrowableNetworkHandler.GetLimitedVelocity(base.Base.Instance.Owner.GetVelocity()));
    }
}