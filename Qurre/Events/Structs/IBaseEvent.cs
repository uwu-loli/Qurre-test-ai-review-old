using JetBrains.Annotations;

namespace Qurre.Events.Structs;

[PublicAPI]
public interface IBaseEvent
{
    uint EventId { get; }
}