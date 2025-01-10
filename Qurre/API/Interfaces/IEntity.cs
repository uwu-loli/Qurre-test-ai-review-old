using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Qurre.API.Interfaces;

[PublicAPI]
public interface IEntity
{
    event Action? Destroyed;

    GameObject GameObject { get; }
    
    bool IsAlive { get; }
    bool IsDestroyed { get; }
    
    string Tag { get; set; }
    
    void Destroy();
}