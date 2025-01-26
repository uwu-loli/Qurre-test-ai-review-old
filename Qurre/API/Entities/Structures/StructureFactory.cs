using System;
using JetBrains.Annotations;
using Qurre.API.Addons;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Entities.Structures;

[PublicAPI]
public static class StructureFactory
{
    #region Generator Factory

    public static IGenerator CreateGenerator(Vector3? position = null,
        Quaternion? rotation = null,
        Vector3? localScale = null,
        bool doSpawn = true)
    {
        if (Prefabs.Generator == null)
            throw new NullReferenceException();

        var generatorBase =
            Object.Instantiate(Prefabs.Generator, position ?? Vector3.zero, rotation ?? Quaternion.identity);
        generatorBase.transform.localScale = localScale ?? Vector3.one;

        var generator = EntityManager.Get<IGenerator>(generatorBase);
        if (generator is null)
            throw new NullReferenceException(nameof(generator));

        if (doSpawn) generator.Spawn();
        return generator;
    }

    #endregion
}