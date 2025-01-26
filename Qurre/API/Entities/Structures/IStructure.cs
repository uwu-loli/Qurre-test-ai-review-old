using JetBrains.Annotations;
using MapGeneration.Distributors;
using Qurre.API.Core;
using StructureBase = MapGeneration.Distributors.SpawnableStructure;

namespace Qurre.API.Entities.Structures;

[PublicAPI]
public interface IStructure : ILevelEntity
{
    UnityObjectWrapper<StructureBase> Base { get; }

    StructureType StructureType { get; }

    int MinAmount { get; }

    int MaxAmount { get; }
}