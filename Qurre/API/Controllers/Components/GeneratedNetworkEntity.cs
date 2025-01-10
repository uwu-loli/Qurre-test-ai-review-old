using System;
using JetBrains.Annotations;
using Mirror;
using Qurre.API.Interfaces;

namespace Qurre.API.Controllers.Components;

[PublicAPI]
public abstract class GeneratedNetworkEntity<TBase, T> : NetworkEntity<TBase, T>, IMapGeneratedEntity
    where TBase : NetworkBehaviour
    where T : NetworkEntity<TBase, T>
{
    public bool IsMapGenerated { get; private set; }
    public bool IsCustom => !IsMapGenerated;
    
    internal void MarkAsMapGenerated() => IsMapGenerated = true;
    
    protected override SpawnMessage GetSpawnMessage(ArraySegment<byte> payloadSegment)
    {
        if (!IsMapGenerated) return base.GetSpawnMessage(payloadSegment);
        return new SpawnMessage
        {
            netId = NetworkIdentity.netId,
            isLocalPlayer = false,
            isOwner = false,
            sceneId = NetworkIdentity.sceneId,
            assetId = NetworkIdentity.assetId,
            position = LocalPosition,
            rotation = LocalRotation,
            scale = LocalScale,
            payload = payloadSegment
        };
    }
}
