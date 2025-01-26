using JetBrains.Annotations;

namespace Qurre.API.Core;

[PublicAPI]
public interface ILevelEntity : INetworkEntity
{
    bool IsLevelGenerated { get; }
}