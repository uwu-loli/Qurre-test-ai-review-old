using System;
using JetBrains.Annotations;
using Qurre.API.Core;
using UnityEngine;

namespace Qurre.Internal.Misc;

public class EntityLink : MonoBehaviour
{
    [PublicAPI] public IEntity? Entity { get; private set; }

    private void OnDestroy()
    {
        ObjectDestroyed?.Invoke();
    }

    public event Action? ObjectDestroyed;

    [PublicAPI]
    public void Setup(IEntity entity)
    {
        if (Entity != null) return;
        Entity = entity;
    }
}