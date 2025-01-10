using System;
using JetBrains.Annotations;

namespace Qurre.API.Interfaces;

[PublicAPI]
public interface IToyEntity : INetworkEntity
{
    event Action IsStaticUpdated;
    event Action MovementSmoothingUpdated;
    
    bool IsStatic { get; set; }
    byte MovementSmoothing { get; set; }
}
