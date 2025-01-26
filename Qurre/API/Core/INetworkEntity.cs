using System;
using JetBrains.Annotations;
using Mirror;
using Qurre.API.Exceptions;

namespace Qurre.API.Core;

/// <summary>
///     Интерфейс, представляющий сущность, которая взаимодействует через сеть.
///     Сущности, реализующие данный интерфейс, могут быть созданы (спаунены) и уничтожены (антиспаунены) в сети,
///     а также обновлять свои данные для всех пользователей или конкретных подключений.
/// </summary>
[PublicAPI]
public interface INetworkEntity : ITransformEntity
{
    #region Network Events

    /// <summary>
    ///     Событие, которое срабатывает после успешного spawn сущности в сети.
    /// </summary>
    event Action Spawned;

    /// <summary>
    ///     Событие, которое срабатывает после успешного un-spawn сущности из сети.
    /// </summary>
    event Action UnSpawned;

    #endregion

    #region Properties

    /// <summary>
    ///     Получает <see cref="NetworkIdentity" />, представляющий уникальный идентификатор сущности в сети.
    /// </summary>
    /// <returns><see cref="NetworkIdentity" />, представляющий уникальный идентификатор сущности в сети.</returns>
    UnityObjectWrapper<NetworkIdentity> NetworkIdentity { get; }

    /// <summary>
    ///     Проверяет, spawned ли сущность в сети.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ObjectDestroyedException" />
    bool IsSpawned { get; }

    #endregion

    #region Methods

    /// <summary>
    ///     Spawns сущности для клиентов.
    ///     Также включает её объект на сервере.
    /// </summary>
    void Spawn();

    /// <summary>
    ///     Un-spawns сущности для клиентов.
    ///     Также выключает её объект на сервере.
    /// </summary>
    void UnSpawn();

    void Respawn();

    /// <summary>
    ///     Обновление данных сущности для всех подключений.
    /// </summary>
    void UpdateDataForAll();

    /// <summary>
    ///     Обновление данных для конкретного подключения в сети.
    /// </summary>
    /// <param name="connection">Сетевое подключение, для которого обновляются данные.</param>
    void UpdateForConnection(NetworkConnection connection);

    /// <summary>
    ///     Генерация сообщения spawn сущности для подключения.
    /// </summary>
    /// <param name="conn">Необязательное подключение, для которого генерируется сообщение. Может быть null.</param>
    /// <returns>Сообщение spawn для подключения.</returns>
    /// <exception cref="ObjectDestroyedException" />
    SpawnMessage GetSpawnMessage(NetworkConnection? conn = null);

    #endregion
}