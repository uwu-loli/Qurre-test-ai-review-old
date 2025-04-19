using Footprinting;
using InventorySystem.Items.ThrowableProjectiles;
using Mirror;
using Qurre.API.Entities.Characters;
using Qurre.Internal.Attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Entities.Items.Implementations;

[EntityWrapBindForFactory(typeof(GrenadeFlash))]
internal sealed class GrenadeFlash : Throwable, IGrenadeFlash
{
    public GrenadeFlash(ThrowableItem itemBase, Player? owner = null) : base(itemBase)
    {
        var grenade = (FlashbangGrenade)Base.Instance.Projectile;
        BlindAnimation = grenade._blindingOverDistance;
        SurfaceDistanceIntensifier = grenade._surfaceZoneDistanceIntensifier;
        DeafenAnimation = grenade._deafenDurationOverDistance;
        FuseTime = grenade._fuseTime;
    }

    public AnimationCurve BlindAnimation { get; set; }
    public float SurfaceDistanceIntensifier { get; set; }
    public AnimationCurve DeafenAnimation { get; set; }
    public float FuseTime { get; set; }

    public new void Spawn(Vector3 position, Quaternion rotation = default, Vector3 scale = default)
    {
        var grenade = (FlashbangGrenade)Object.Instantiate(Base.Instance.Projectile, position, rotation);

        grenade.PreviousOwner = new Footprint(Owner.ReferenceHub);
        grenade._blindingOverDistance = BlindAnimation;
        grenade._surfaceZoneDistanceIntensifier = SurfaceDistanceIntensifier;
        grenade._deafenDurationOverDistance = DeafenAnimation;
        grenade._fuseTime = FuseTime;

        if (scale != default)
            grenade.transform.localScale = scale;

        NetworkServer.Spawn(grenade.gameObject);
        grenade.ServerActivate();
    }
}