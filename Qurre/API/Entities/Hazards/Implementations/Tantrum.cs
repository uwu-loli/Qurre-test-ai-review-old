using Qurre.API.Core;
using Qurre.Internal.Attributes;
using TantrumBase = Hazards.TantrumEnvironmentalHazard;

namespace Qurre.API.Entities.Hazards.Implementations;

[EntityWrapBindForFactory(typeof(TantrumBase))]
internal sealed class Tantrum(TantrumBase tantrumBase)
    : TemporaryHazard(tantrumBase), ITantrum
{
    /// <inheritdoc />
    public new UnityObjectWrapper<TantrumBase> Base { get; } = tantrumBase;

    /// <inheritdoc />
    public bool PlaySizzle
    {
        get => Base.Instance.PlaySizzle;
        set => Base.Instance.PlaySizzle = value;
    }
}