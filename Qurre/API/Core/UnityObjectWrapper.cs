using JetBrains.Annotations;
using Qurre.API.Exceptions;
using UnityEngine;

namespace Qurre.API.Core;

[PublicAPI]
public class UnityObjectWrapper<T>(T instance) where T : Object
{
    public bool IsAlive => (bool)instance;

    /// <exception cref="ObjectDestroyedException">
    ///     Выбрасывается, если объект был уничтожен, и доступ к нему невозможен.
    /// </exception>
    public T Instance => IsAlive
        ? instance
        : throw new ObjectDestroyedException(typeof(T).Name);

    public static explicit operator T(UnityObjectWrapper<T> wrapper)
    {
        return wrapper.Instance;
    }

    public static implicit operator UnityObjectWrapper<T>(T instance)
    {
        return new UnityObjectWrapper<T>(instance);
    }
}