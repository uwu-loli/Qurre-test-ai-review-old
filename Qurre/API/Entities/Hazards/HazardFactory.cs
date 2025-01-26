using System;
using JetBrains.Annotations;
using Qurre.API.Addons;
using Qurre.API.Entities.Hazards.Implementations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Entities.Hazards;

[PublicAPI]
public class HazardFactory
{
    #region Tantrum Factory

    public ITantrum CreateTantrum(Vector3 position,
        Quaternion? rotation = null,
        Vector3? scale = null,
        bool doSpawn = true)
    {
        if (Prefabs.Tantrum is null)
            throw new NullReferenceException("Prefab.Tantrum is null.");
        
        var spawnRotation = rotation ?? Quaternion.identity;
        var spawnScale = scale ?? Vector3.one;

        var tantrumInstance = Object.Instantiate(Prefabs.Tantrum, position, spawnRotation);
        tantrumInstance.transform.localScale = spawnScale;

        var tantrum = new Tantrum(tantrumInstance);
        if (doSpawn) tantrum.Spawn();
        else tantrum.UnSpawn();
        return tantrum;
    }

    public ITantrum CreateTantrum(Vector3 position,
        Vector3 eulerAngles,
        Vector3? scale = null,
        bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreateTantrum(position, rotationQuaternion, scale, doSpawn);
    }

    #endregion
}