using MapGeneration.Distributors;
using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.Internal.Attributes;
using StructureBase = MapGeneration.Distributors.SpawnableStructure;

namespace Qurre.API.Entities.Structures.Implementations;

[EntityWrapBindForFactory(typeof(StructureBase))]
internal abstract class StructureEntity(StructureBase structureBase) : LevelEntity(structureBase.gameObject), IStructure
{
    private readonly UnityObjectWrapper<StructurePositionSync> _positionSync =
        structureBase.GetComponent<StructurePositionSync>();

    /// <inheritdoc />
    public UnityObjectWrapper<StructureBase> Base { get; } = structureBase;

    /// <inheritdoc />
    public StructureType StructureType => Base.Instance.StructureType;

    /// <inheritdoc />
    public int MinAmount => Base.Instance.MinAmount;

    /// <inheritdoc />
    public int MaxAmount => Base.Instance.MaxAmount;

    /// <inheritdoc />
    protected sealed override void OnPositionChanged()
    {
        base.OnPositionChanged();
        if (!_positionSync.IsAlive) return;
        _positionSync.Instance.Network_rotationY = (sbyte)(WorldEulerAngles.y / 5.625f);
    }

    /// <inheritdoc />
    protected sealed override void OnRotationChanged()
    {
        base.OnRotationChanged();
        if (!_positionSync.IsAlive) return;
        _positionSync.Instance.Network_position = WorldPosition;
    }
}