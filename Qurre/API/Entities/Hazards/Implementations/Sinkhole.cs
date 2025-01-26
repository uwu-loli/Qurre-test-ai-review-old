using Qurre.API.Core;
using Qurre.Internal.Attributes;
using SinkholeBase = Hazards.SinkholeEnvironmentalHazard;

namespace Qurre.API.Entities.Hazards.Implementations;

[EntityWrapBindForFactory(typeof(SinkholeBase))]
internal sealed class Sinkhole(SinkholeBase sinkholeBase)
    : Hazard(sinkholeBase), ISinkhole
{
    /// <inheritdoc />
    public new UnityObjectWrapper<SinkholeBase> Base { get; } = sinkholeBase;
}