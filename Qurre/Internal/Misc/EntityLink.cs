using System;
using JetBrains.Annotations;
using Qurre.API.Core;
using UnityEngine;

namespace Qurre.Internal.Misc;

public class EntityLink : MonoBehaviour
{
    public event Action? ObjectDestroyed;
    
    [PublicAPI]
    public IEntity? Entity { get; private set; }

    [PublicAPI]
    public void Setup(IEntity entity)
    {
        if (Entity != null) return;
        Entity = entity;
    }

    private void OnDestroy()
    {
        ObjectDestroyed?.Invoke();
    }
}
