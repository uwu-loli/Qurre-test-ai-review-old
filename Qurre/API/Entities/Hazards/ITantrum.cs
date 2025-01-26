using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using TantrumBase = Hazards.TantrumEnvironmentalHazard;

namespace Qurre.API.Entities.Hazards;

[PublicAPI]
public interface ITantrum : ITemporaryHazard
{
    new UnityObjectWrapper<TantrumBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    bool PlaySizzle { get; set; }
}