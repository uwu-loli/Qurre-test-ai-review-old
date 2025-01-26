using JetBrains.Annotations;

namespace Qurre.API.Enums;

[PublicAPI]
public enum RadioStatus : sbyte
{
    Disabled = -1,
    Short,
    Medium,
    Long,
    Ultra
}
