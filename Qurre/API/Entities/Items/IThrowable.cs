using InventorySystem.Items.ThrowableProjectiles;
using JetBrains.Annotations;
using Qurre.API.Core;

namespace Qurre.API.Entities.Items;

[PublicAPI]
public interface IThrowable : IItem
{
    new UnityObjectWrapper<ThrowableItem> Base { get; }
    UnityObjectWrapper<ThrownProjectile> Projectile { get; }

    float PinPullTime { get; set; }
    void Throw(bool fullForce = true);
}