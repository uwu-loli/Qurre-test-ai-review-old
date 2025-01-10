using System.Diagnostics.CodeAnalysis;
using Hazards;
using JetBrains.Annotations;
using Qurre.API.Controllers.Components;

namespace Qurre.API.Controllers;

[PublicAPI]
public class Sinkhole : GeneratedNetworkEntity<SinkholeEnvironmentalHazard, Sinkhole>
{
    protected override SinkholeEnvironmentalHazard UnsafeBase { get; }
    
    private Sinkhole(SinkholeEnvironmentalHazard sinkholeBase)
    {
        UnsafeBase = sinkholeBase;

        BaseToWrap[Base] = this;
        AddEntityLink();
    }

    public bool ImmunityScps { get; set; }

    public string Name => GameObject.name;

    public float MaxDistance
    {
        get => Base.MaxDistance;
        set => Base.MaxDistance = value;
    }

    public float MaxHeightDistance
    {
        get => Base.MaxHeightDistance;
        set => Base.MaxHeightDistance = value;
    }

    public static Sinkhole? Get(SinkholeEnvironmentalHazard sinkholeBase)
    {
        if (!sinkholeBase) return null;
        return BaseToWrap.TryGetValue(sinkholeBase, out var sinkhole) ? sinkhole : new Sinkhole(sinkholeBase);
    }

    public static bool TryGet(SinkholeEnvironmentalHazard sinkholeBase, [NotNullWhen(true)] out Sinkhole? sinkhole)
    {
        sinkhole = Get(sinkholeBase);
        return sinkhole is not null;
    }
}