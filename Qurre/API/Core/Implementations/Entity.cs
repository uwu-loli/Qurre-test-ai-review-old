using System;
using JetBrains.Annotations;
using Qurre.API.Addons;
using Qurre.API.Exceptions;
using Qurre.Internal.Misc;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Core.Implementations;

[MeansImplicitUse(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal abstract class Entity : IEntity
{
    /// <exception cref="ObjectDestroyedException" />
    protected Entity(GameObject gameObject)
    {
        GameObject = gameObject;
        var entityLink = GameObject.Instance.AddComponent<EntityLink>();
        entityLink.ObjectDestroyed += OnObjectDestroyed;
        entityLink.Setup(this);
    }

    /// <inheritdoc />
    public event Action? Destroyed;

    /// <inheritdoc />
    public UnityObjectWrapper<GameObject> GameObject { get; }

    /// <inheritdoc />
    public VariableDictionary<object> Variables { get; } = [];

    /// <inheritdoc />
    public void Destroy()
    {
        if (!GameObject.IsAlive) return;
        Object.Destroy(GameObject.Instance);
    }

    private void OnObjectDestroyed()
    {
        Destroyed?.Invoke();
        Destroyed = null;
    }
}