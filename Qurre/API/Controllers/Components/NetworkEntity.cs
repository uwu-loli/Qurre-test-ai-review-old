using System;
using JetBrains.Annotations;
using Mirror;
using Qurre.API.Interfaces;

namespace Qurre.API.Controllers.Components;

[PublicAPI]
public abstract class NetworkEntity<TBase, T> : TransformableEntity<TBase, T>, INetworkEntity
    where TBase : NetworkBehaviour
    where T : NetworkEntity<TBase, T>
{
    public event Action? Spawned;
    public event Action? UnSpawned;

    public NetworkIdentity NetworkIdentity => Base.netIdentity;

    public uint NetworkId => NetworkIdentity.netId;

    public bool IsSpawned => NetworkServer.spawned.ContainsKey(NetworkIdentity.netId);
    public bool IsUnSpawned => !IsSpawned;
    
    protected NetworkEntity()
    {
        PositionUpdated += OnPositionUpdated;
        RotationUpdated += OnRotationUpdated;
        ScaleUpdated += OnScaleUpdated;
        
        Destroyed += OnDestroyed;
    }

    public void Spawn()
    {
        NetworkServer.Spawn(GameObject);
        Spawned?.Invoke();
    }

    public void UnSpawn()
    {
        NetworkServer.UnSpawn(GameObject);
        UnSpawned?.Invoke();
    }

    public void Respawn()
    {
        UnSpawn();
        Spawn();
    }
    
    public void NetworkUpdateForAll()
    {
        if (IsUnSpawned) return;
        var spawnMessage = GetSpawnMessage();
        NetworkServer.SendToAll(spawnMessage);
    }

    public void NetworkUpdateForConnection(NetworkConnectionToClient connectionToClient)
    {
        if (IsUnSpawned) return;
        var spawnMessage = GetSpawnMessage();
        connectionToClient.SendDataToClient(spawnMessage);
    }

    private void OnDestroyed()
    {
        if (IsAlive) NetworkServer.Destroy(GameObject);
    }

    protected virtual void OnPositionUpdated() => NetworkUpdateForAll();
    protected virtual void OnRotationUpdated() => NetworkUpdateForAll();
    protected virtual void OnScaleUpdated() => NetworkUpdateForAll();

    protected SpawnMessage GetSpawnMessage()
    {
        using var ownerWriter = NetworkWriterPool.Get();
        using var observersWriter = NetworkWriterPool.Get();
        var payloadSegment = NetworkServer.CreateSpawnMessagePayload(isOwner: false, NetworkIdentity, ownerWriter, observersWriter);

        return GetSpawnMessage(payloadSegment);
    }

    protected virtual SpawnMessage GetSpawnMessage(ArraySegment<byte> payloadSegment)
    {
        return new SpawnMessage
        {
            netId = NetworkIdentity.netId,
            isLocalPlayer = false,
            isOwner = false,
            sceneId = NetworkIdentity.sceneId,
            assetId = NetworkIdentity.assetId,
            position = WorldPosition,
            rotation = WorldRotation,
            scale = LocalScale,
            payload = payloadSegment
        };
    }
}
