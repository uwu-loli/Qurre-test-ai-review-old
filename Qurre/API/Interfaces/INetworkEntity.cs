using System;
using JetBrains.Annotations;

namespace Qurre.API.Interfaces;

[PublicAPI]
public interface INetworkEntity : ITransformableEntity
{
    event Action Spawned;
    event Action UnSpawned;
    
    bool IsSpawned { get; }
    bool IsUnSpawned { get; }
    
    void Spawn();
    void UnSpawn();
    void Respawn();
}
