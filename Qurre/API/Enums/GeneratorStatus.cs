using JetBrains.Annotations;

namespace Qurre.API.Enums;

[PublicAPI]
public enum GeneratorStatus : byte
{
    Activate,
    Deactivate,
    Unlock,
    OpenDoor,
    CloseDoor
}
