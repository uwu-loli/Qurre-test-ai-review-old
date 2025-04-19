using JetBrains.Annotations;

namespace Qurre.API.Enums;

[PublicAPI]
public enum LockerTypes : byte
{
    Unknown,
    AdrenalineMedkit,
    RegularMedkit,
    Pedestal,
    MiscLocker,
    RifleRack,
    LargeGun
}