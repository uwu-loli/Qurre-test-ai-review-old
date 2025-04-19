using JetBrains.Annotations;

namespace Qurre.API.Enums;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
[PublicAPI]
public enum DamagePrimitiveTypes : byte
{
    Unknown,
    Custom,
    Disruptor,
    Explosion,
    Gun,
    MicroHid,
    Recontainment,
    Scp018,
    Scp049,
    Scp096,
    ScpDamage,
    Universal,
    Warhead,
    Jailbird,
    Snowball
}