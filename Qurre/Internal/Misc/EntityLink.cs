using JetBrains.Annotations;
using Qurre.API.Interfaces;
using UnityEngine;

namespace Qurre.Internal.Misc;

internal class EntityLink : MonoBehaviour
{
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
        Entity?.Destroy();
    }
}
