using JetBrains.Annotations;
using Qurre.API.Entities.AdminToys;
using Qurre.API.Entities.Characters;

namespace Qurre.API.Entities;

[PublicAPI]
public static class EntityFactories
{
    public static AdminToyFactory AdminToy { get; } = new();
    public static CharacterFactory Character { get; } = new();
}