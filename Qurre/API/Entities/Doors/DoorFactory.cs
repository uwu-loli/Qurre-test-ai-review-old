using System.Collections.Generic;
using JetBrains.Annotations;
using Qurre.API.Addons;
using Qurre.API.Enums;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Entities.Doors;

[PublicAPI]
public class DoorFactory
{
    #region Breakable Door Factory

    public IBreakableDoor CreateDoor(DoorPrefabs prefab,
        Vector3 position,
        Quaternion? rotation = null,
        Vector3? scale = null,
        bool doSpawn = true)
    {
        if (!Prefabs.Doors.TryGetValue(prefab, out var doorPrefab))
            throw new KeyNotFoundException($"Prefabs.Doors[{prefab}] not found.");

        var spawnRotation = rotation ?? Quaternion.identity;
        var spawnScale = scale ?? Vector3.one;

        var doorInstance = Object.Instantiate(doorPrefab, position, spawnRotation);
        doorInstance.transform.localScale = spawnScale;

        var door = EntityManager.GetOrException<IBreakableDoor>(doorInstance);
        if (doSpawn) door.Spawn();
        else door.UnSpawn();
        return door;
    }

    public IBreakableDoor CreateDoor(DoorPrefabs prefab,
        Vector3 position,
        Vector3 eulerAngles,
        Vector3? scale = null,
        bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreateDoor(prefab, position, rotationQuaternion, scale, doSpawn);
    }

    #endregion
}