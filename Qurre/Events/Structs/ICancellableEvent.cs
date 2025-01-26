using JetBrains.Annotations;

namespace Qurre.Events.Structs;

[PublicAPI]
public interface ICancellableEvent : IBaseEvent
{
    bool IsAllowed { get; set; }
}
