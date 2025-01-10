using JetBrains.Annotations;

namespace Qurre.API.Interfaces;

[PublicAPI]
public interface IMapGeneratedEntity : ITransformableEntity
{
    bool IsMapGenerated { get; }
    bool IsCustom { get; }
}
