using System;
using JetBrains.Annotations;
using Qurre.API.Exceptions;
using UnityEngine;

namespace Qurre.API.Core;

/// <summary>
///     Интерфейс для доступа к данным трансформации сущности в игровом мире.
///     Позволяет получить или задать глобальные и локальные трансформации (позицию, вращение, масштаб) сущности.
///     Обеспечивает уведомления об изменениях трансформаций.
/// </summary>
[PublicAPI]
public interface ITransformEntity : IReadOnlyTransformEntity
{
    #region Transform Change Events

    /// <summary>
    ///     Событие, которое срабатывает при изменении позиции сущности.
    /// </summary>
    event Action PositionChanged;

    /// <summary>
    ///     Событие, которое срабатывает при изменении вращения сущности.
    /// </summary>
    event Action RotationChanged;

    /// <summary>
    ///     Событие, которое срабатывает при изменении масштаба сущности.
    /// </summary>
    event Action ScaleChanged;

    #endregion

    #region Global Transform

    /// <summary>
    ///     Получает или задаёт глобальную позицию сущности в игровом мире.
    /// </summary>
    /// <returns>Глобальная позиция сущности.</returns>
    /// <exception cref="ObjectDestroyedException">
    ///     Выбрасывается, если сущность была уничтожена, и доступ к объекту невозможен.
    /// </exception>
    new Vector3 WorldPosition { get; set; }

    /// <summary>
    ///     Получает или задаёт глобальное вращение сущности в виде кватерниона.
    /// </summary>
    /// <returns>Глобальное вращение сущности в виде кватерниона.</returns>
    /// <exception cref="ObjectDestroyedException">
    ///     Выбрасывается, если сущность была уничтожена, и доступ к объекту невозможен.
    /// </exception>
    new Quaternion WorldRotation { get; set; }

    /// <summary>
    ///     Получает или задаёт глобальное вращение сущности в виде углов Эйлера.
    /// </summary>
    /// <returns>Глобальные углы Эйлера сущности.</returns>
    /// <exception cref="ObjectDestroyedException">
    ///     Выбрасывается, если сущность была уничтожена, и доступ к объекту невозможен.
    /// </exception>
    new Vector3 WorldEulerAngles { get; set; }

    /// <summary>
    ///     Получает или задаёт глобальный масштаб сущности.
    /// </summary>
    /// <returns>Глобальный масштаб сущности.</returns>
    /// <exception cref="ObjectDestroyedException">
    ///     Выбрасывается, если сущность была уничтожена, и доступ к объекту невозможен.
    /// </exception>
    new Vector3 WorldScale { get; set; }

    #endregion

    #region Local Transform

    /// <summary>
    ///     Получает или задаёт локальную позицию сущности относительно её родительского объекта.
    /// </summary>
    /// <returns>Локальная позиция сущности относительно родителя.</returns>
    /// <exception cref="ObjectDestroyedException">
    ///     Выбрасывается, если сущность была уничтожена, и доступ к объекту невозможен.
    /// </exception>
    new Vector3 LocalPosition { get; set; }

    /// <summary>
    ///     Получает или задаёт локальное вращение сущности относительно её родительского объекта.
    /// </summary>
    /// <returns>Локальное вращение сущности относительно родителя в виде кватерниона.</returns>
    /// <exception cref="ObjectDestroyedException">
    ///     Выбрасывается, если сущность была уничтожена, и доступ к объекту невозможен.
    /// </exception>
    new Quaternion LocalRotation { get; set; }

    /// <summary>
    ///     Получает или задаёт локальное вращение сущности относительно её родительского объекта в виде углов Эйлера.
    /// </summary>
    /// <returns>Локальные углы Эйлера сущности относительно родителя.</returns>
    /// <exception cref="ObjectDestroyedException">
    ///     Выбрасывается, если сущность была уничтожена, и доступ к объекту невозможен.
    /// </exception>
    new Vector3 LocalEulerAngles { get; set; }

    /// <summary>
    ///     Получает или задаёт локальный масштаб сущности относительно её родительского объекта.
    /// </summary>
    /// <returns>Локальный масштаб сущности относительно родителя.</returns>
    /// <exception cref="ObjectDestroyedException">
    ///     Выбрасывается, если сущность была уничтожена, и доступ к объекту невозможен.
    /// </exception>
    new Vector3 LocalScale { get; set; }

    #endregion
}