using JetBrains.Annotations;
using Qurre.API.Core;
using SinkholeBase = Hazards.SinkholeEnvironmentalHazard;

namespace Qurre.API.Entities.Hazards;

[PublicAPI]
public interface ISinkhole : IHazard
{
    new UnityObjectWrapper<SinkholeBase> Base { get; }
}