using System;
using JetBrains.Annotations;
using Mirror;
using Qurre.API.Exceptions;
using UnityEngine;

namespace Qurre.API.Core.Implementations;

[MeansImplicitUse(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal abstract class NetworkEntity : TransformEntity, INetworkEntity
{
    /// <exception cref="ObjectDestroyedException" />
    protected NetworkEntity(GameObject gameObject) : base(gameObject)
    {
        NetworkIdentity = GameObject.Instance.GetComponent<NetworkIdentity>();
        PositionChanged += OnPositionChanged;
        RotationChanged += OnRotationChanged;
        ScaleChanged += OnScaleChanged;
    }

    /// <inheritdoc />
    public event Action? Spawned;

    /// <inheritdoc />
    public event Action? UnSpawned;

    /// <inheritdoc />
    public UnityObjectWrapper<NetworkIdentity> NetworkIdentity { get; }

    /// <inheritdoc />
    public bool IsSpawned => NetworkServer.spawned.ContainsKey(NetworkIdentity.Instance.netId);

    /// <inheritdoc />
    public void Spawn()
    {
        GameObject.Instance.SetActive(true);
        if (!NetworkIdentity.IsAlive || IsSpawned) return;
        NetworkServer.Spawn(GameObject.Instance);
        Spawned?.Invoke();
    }

    /// <inheritdoc />
    public void UnSpawn()
    {
        GameObject.Instance.SetActive(false);
        if (!NetworkIdentity.IsAlive || !IsSpawned) return;
        NetworkServer.UnSpawn(GameObject.Instance);
        UnSpawned?.Invoke();
    }

    /// <inheritdoc />
    public void Respawn()
    {
        if (!NetworkIdentity.IsAlive) return;
        UnSpawn();
        Spawn();
    }

    /// <inheritdoc />
    public void UpdateDataForAll()
    {
        if (!NetworkIdentity.IsAlive) return;
        foreach (var connection in NetworkServer.connections.Values) UpdateForConnection(connection);
    }

    /// <inheritdoc />
    public void UpdateForConnection(NetworkConnection connection)
    {
        if (!NetworkIdentity.IsAlive) return;
        if (!connection.isReady) return;
        var spawnMessage = GetSpawnMessage(connection);
        var arraySegment = SerializeMessage(spawnMessage);
        connection.Send(arraySegment);
    }

    /// <inheritdoc />
    public virtual SpawnMessage GetSpawnMessage(NetworkConnection? conn = null)
    {
        // Don't replace Worlds with Locals here!
        // Locals are managed by LevelEntity!

        return new SpawnMessage
        {
            netId = NetworkIdentity.Instance.netId,
            isLocalPlayer = false,
            isOwner = false,
            sceneId = NetworkIdentity.Instance.sceneId,
            assetId = NetworkIdentity.Instance.assetId,
            position = WorldPosition,
            rotation = WorldRotation,
            scale = WorldScale,
            payload = GetSpawnMessagePayload()
        };
    }

    protected ArraySegment<byte> GetSpawnMessagePayload()
    {
        using var ownerWriter = NetworkWriterPool.Get();
        using var observersWriter = NetworkWriterPool.Get();
        return NetworkServer.CreateSpawnMessagePayload(false, NetworkIdentity.Instance, ownerWriter, observersWriter);
    }

    private static ArraySegment<byte> SerializeMessage<T>(T message) where T : struct, NetworkMessage
    {
        using var writer = NetworkWriterPool.Get();
        NetworkMessages.Pack(message, writer);
        return writer.ToArraySegment();
    }

    /// <summary>
    ///     Метод, вызываемый при изменении позиции сущности.
    ///     По умолчанию вызывает <see cref="UpdateDataForAll" />.
    /// </summary>
    protected virtual void OnPositionChanged()
    {
        UpdateDataForAll();
    }

    /// <summary>
    ///     Метод, вызываемый при изменении поворота сущности.
    ///     По умолчанию вызывает <see cref="UpdateDataForAll" />.
    /// </summary>
    protected virtual void OnRotationChanged()
    {
        UpdateDataForAll();
    }

    /// <summary>
    ///     Метод, вызываемый при изменении размера сущности.
    ///     По умолчанию вызывает <see cref="UpdateDataForAll" />.
    /// </summary>
    protected virtual void OnScaleChanged()
    {
        UpdateDataForAll();
    }
}