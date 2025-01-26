using JetBrains.Annotations;

namespace Qurre.API.Enums;

[PublicAPI]
public enum WorkstationStatus : byte
{
    Offline,
    PoweringUp,
    PoweringDown,
    Online
}
