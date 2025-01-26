using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using TemporaryHazardBase = Hazards.TemporaryHazard;

namespace Qurre.API.Entities.Hazards;

[PublicAPI]
public interface ITemporaryHazard : IHazard
{
    new UnityObjectWrapper<TemporaryHazardBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    float RemainingLifetime { get; }

    /// <exception cref="ObjectDestroyedException" />
    float LifecycleDuration { get; }

    /// <exception cref="ObjectDestroyedException" />
    float Lifetime { get; }

    /// <exception cref="ObjectDestroyedException" />
    float DecaySpeed { get; }
}