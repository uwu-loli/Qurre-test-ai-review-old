using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Qurre.API.Addons;
using Qurre.API.Enums;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Entities.Structures;

[PublicAPI]
public class StructureFactory
{
    #region Generator Factory

    public IGenerator CreateGenerator(Vector3 position,
        Quaternion? rotation = null,
        Vector3? scale = null,
        bool doSpawn = true)
    {
        if (Prefabs.Generator == null)
            throw new NullReferenceException("Prefabs.Generator is null.");
        
        var spawnRotation = rotation ?? Quaternion.identity;
        var spawnScale = scale ?? Vector3.one;

        var generatorInstance = Object.Instantiate(Prefabs.Generator, position, spawnRotation);
        generatorInstance.transform.localScale = spawnScale;

        var generator = EntityManager.GetOrException<IGenerator>(generatorInstance);
        if (doSpawn) generator.Spawn();
        else generator.UnSpawn();
        return generator;
    }


    public IGenerator CreateGenerator(Vector3 position,
        Vector3 eulerAngles,
        Vector3? scale = null,
        bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreateGenerator(position, rotationQuaternion, scale, doSpawn);
    }

    #endregion
    
    #region Generator Factory

    public ILocker CreateLocker(LockerPrefabs prefab,
        Vector3 position,
        Quaternion? rotation = null,
        Vector3? scale = null,
        bool doSpawn = true)
    {
        if (!Prefabs.Lockers.TryGetValue(prefab, out var lockerPrefab))
            throw new KeyNotFoundException($"Prefabs.Lockers[{prefab}] not found.");
        
        var spawnRotation = rotation ?? Quaternion.identity;
        var spawnScale = scale ?? Vector3.one;

        var lockerInstance = Object.Instantiate(lockerPrefab, position, spawnRotation);
        lockerInstance.transform.localScale = spawnScale;

        var locker = EntityManager.GetOrException<ILocker>(lockerInstance);
        if (doSpawn) locker.Spawn();
        else locker.UnSpawn();
        return locker;
    }


    public ILocker CreateLocker(LockerPrefabs prefab,
        Vector3 position,
        Vector3 eulerAngles,
        Vector3? scale = null,
        bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreateLocker(prefab, position, rotationQuaternion, scale, doSpawn);
    }

    #endregion
}