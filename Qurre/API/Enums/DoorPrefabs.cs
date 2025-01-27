using JetBrains.Annotations;

namespace Qurre.API.Enums;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

[PublicAPI]
public enum DoorPrefabs : byte
{
    Unknown,
    DoorLCZ,
    DoorHCZ,
    DoorEZ,
    BulkHCZ
}
