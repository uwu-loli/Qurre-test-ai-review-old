using JetBrains.Annotations;
using Qurre.API.Exceptions;
using UnityEngine;

namespace Qurre.API.Core;

/// <summary>
///     Интерфейс для read-only доступа к данным трансформации сущности в игровом мире.
///     Позволяет получить глобальные и локальные трансформации (позицию, вращение, масштаб) сущности.
/// </summary>
[PublicAPI]
public interface IReadOnlyTransformEntity : IEntity
{
    #region Global Transform

    /// <summary>
    ///     Получает глобальную позицию сущности в игровом мире.
    /// </summary>
    /// <returns>Глобальная позиция сущности.</returns>
    /// <exception cref="ObjectDestroyedException" />
    Vector3 WorldPosition { get; }

    /// <summary>
    ///     Получает глобальное вращение сущности в виде кватерниона.
    /// </summary>
    /// <returns>Глобальное вращение сущности в виде кватерниона.</returns>
    /// <exception cref="ObjectDestroyedException" />
    Quaternion WorldRotation { get; }

    /// <summary>
    ///     Получает глобальное вращение сущности в виде углов Эйлера.
    /// </summary>
    /// <returns>Глобальные углы Эйлера сущности.</returns>
    /// <exception cref="ObjectDestroyedException" />
    Vector3 WorldEulerAngles { get; }

    /// <summary>
    ///     Получает глобальный масштаб сущности.
    /// </summary>
    /// <returns>Глобальный масштаб сущности.</returns>
    /// <exception cref="ObjectDestroyedException" />
    Vector3 WorldScale { get; }

    #endregion

    #region Local Transform (Relative to Parent)

    /// <summary>
    ///     Получает локальную позицию сущности относительно её родительского объекта.
    /// </summary>
    /// <returns>Локальная позиция сущности относительно родителя.</returns>
    /// <exception cref="ObjectDestroyedException" />
    Vector3 LocalPosition { get; }

    /// <summary>
    ///     Получает локальное вращение сущности относительно её родительского объекта.
    /// </summary>
    /// <returns>Локальное вращение сущности относительно родителя в виде кватерниона.</returns>
    /// <exception cref="ObjectDestroyedException" />
    Quaternion LocalRotation { get; }

    /// <summary>
    ///     Получает локальное вращение сущности относительно её родительского объекта в виде углов Эйлера.
    /// </summary>
    /// <returns>Локальные углы Эйлера сущности относительно родителя.</returns>
    /// <exception cref="ObjectDestroyedException" />
    Vector3 LocalEulerAngles { get; }

    /// <summary>
    ///     Получает локальный масштаб сущности относительно её родительского объекта.
    /// </summary>
    /// <returns>Локальный масштаб сущности относительно родителя.</returns>
    /// <exception cref="ObjectDestroyedException" />
    Vector3 LocalScale { get; }

    #endregion
}