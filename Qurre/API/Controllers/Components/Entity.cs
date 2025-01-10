using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Qurre.API.Exceptions;
using Qurre.API.Interfaces;
using Qurre.Internal.Misc;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Controllers.Components;

[PublicAPI]
public abstract class Entity<TBase, T> : IEntity
    where TBase : MonoBehaviour
    where T : Entity<TBase, T>
{
    public static IEnumerable<T> List => BaseToWrap.Values;
    protected static readonly Dictionary<TBase, T> BaseToWrap = [];
    
    public event Action? Destroyed;
    
    public string Tag { get; set; } = string.Empty;

    protected abstract TBase UnsafeBase { get; }

    public bool IsAlive => (bool)UnsafeBase;
    public bool IsDestroyed => !IsAlive;

    public TBase Base => IsAlive ? UnsafeBase : throw new ObjectDestroyedException();

    public GameObject GameObject => Base.gameObject;

    public void Destroy()
    {
        if (IsDestroyed) return;
        BaseToWrap.Remove(Base);
        Destroyed?.Invoke();
        Destroyed = null;
        Object.Destroy(GameObject);
    }

    protected void AddEntityLink()
    {
        if (IsDestroyed || (bool)GameObject.GetComponent<EntityLink>()) return;
        var entityLink = GameObject.AddComponent<EntityLink>();
        entityLink.Setup(this);
    }

    internal static void ClearCache()
    {
        BaseToWrap.Clear();
    }
}
