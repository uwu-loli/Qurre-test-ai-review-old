using JetBrains.Annotations;
using Mirror;
using UnityEngine;

namespace Qurre.API.Core.Implementations;

[MeansImplicitUse(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal abstract class LevelEntity(GameObject gameObject) : NetworkEntity(gameObject), ILevelEntity
{
    /// <inheritdoc />
    public bool IsLevelGenerated { get; private set; }

    /// <inheritdoc />
    public override SpawnMessage GetSpawnMessage(NetworkConnection? conn = null)
    {
        if (!IsLevelGenerated)
            return base.GetSpawnMessage(conn);

        // Если объект сгенерирован игрой, то на клиенте у него есть свой Transform.parent,
        // поэтому ему нельзя назначать свой Transform.parent и надо передавать локальную трансформацию

        return new SpawnMessage
        {
            netId = NetworkIdentity.Instance.netId,
            isLocalPlayer = false,
            isOwner = false,
            sceneId = NetworkIdentity.Instance.sceneId,
            assetId = NetworkIdentity.Instance.assetId,
            position = LocalPosition,
            rotation = LocalRotation,
            scale = LocalScale,
            payload = GetSpawnMessagePayload()
        };
    }

    internal void MarkAsLevelGenerated()
    {
        IsLevelGenerated = true;
    }
}