using System;
using AdminToys;
using Footprinting;
using JetBrains.Annotations;
using Qurre.API.Addons;
using Qurre.API.Enums;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Entities.AdminToys;

[PublicAPI]
public class AdminToyFactory
{
    #region Light Point Factory

    public ILightPoint CreateLightPoint(Vector3 position, Quaternion? rotation = null, Color? color = null,
        LightType lightType = LightType.Point, float range = 10, float intensity = 1, float shadowStrength = 0,
        LightShadows shadowType = LightShadows.Hard, float spotAngle = 30, float spotInnerAngle = 0, byte smoothing = 0,
        bool isStatic = false, Footprint? ownerFootprint = null, bool doSpawn = true)
    {
        if (Prefabs.Light == null)
            throw new NullReferenceException("Prefabs.LightPoint not found");

        var spawnRotation = rotation ?? Quaternion.identity;
        var toyInstance = Object.Instantiate(Prefabs.Light, position, spawnRotation);

        toyInstance.SpawnerFootprint = ownerFootprint ?? new Footprint(Server.Host.ReferenceHub);
        toyInstance.NetworkPosition = position;
        toyInstance.NetworkRotation = spawnRotation;
        toyInstance.NetworkMovementSmoothing = smoothing;
        toyInstance.NetworkIsStatic = isStatic;
        
        toyInstance.NetworkLightColor = color ?? Color.white;
        toyInstance.NetworkLightType = lightType;
        toyInstance.NetworkLightRange = range;
        toyInstance.NetworkLightIntensity = intensity;
        toyInstance.NetworkShadowStrength = shadowStrength;
        toyInstance.NetworkShadowType = shadowType;
        toyInstance.NetworkSpotAngle = spotAngle;
        toyInstance.NetworkInnerSpotAngle = spotInnerAngle;

        var lightPoint = EntityManager.Get<ILightPoint>(toyInstance);
        if (lightPoint is null)
            throw new NullReferenceException(nameof(lightPoint));

        if (doSpawn) lightPoint.Spawn();
        return lightPoint;
    }

    public ILightPoint CreateLightPoint(Vector3 position, Vector3 eulerAngles, Color? color = null,
        LightType lightType = LightType.Spot, float range = 10, float intensity = 1, float shadowStrength = 0,
        LightShadows shadowType = LightShadows.Hard, float spotAngle = 30, float spotInnerAngle = 0, byte smoothing = 0,
        bool isStatic = false, Footprint? ownerFootprint = null, bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreateLightPoint(position, rotationQuaternion, color, lightType, range, intensity, shadowStrength,
            shadowType,
            spotAngle, spotInnerAngle, smoothing, isStatic, ownerFootprint, doSpawn);
    }

    #endregion

    #region Primitive Factory

    public IPrimitive CreatePrimitive(Vector3 position, Quaternion? rotation = null, Vector3? scale = null,
        PrimitiveType primitiveType = PrimitiveType.Sphere, Color? materialColor = null,
        PrimitiveFlags primitiveFlags = PrimitiveFlags.Visible | PrimitiveFlags.Collidable, byte smoothing = 0,
        bool isStatic = false, Footprint? ownerFootprint = null, bool doSpawn = true)
    {
        if (Prefabs.Primitive == null)
            throw new NullReferenceException("Prefabs.Primitive not found");

        var spawnRotation = rotation ?? Quaternion.identity;
        var spawnScale = scale ?? Vector3.one;

        var toyInstance = Object.Instantiate(Prefabs.Primitive, position, spawnRotation);
        toyInstance.transform.localScale = spawnScale;

        toyInstance.SpawnerFootprint = ownerFootprint ?? new Footprint(Server.Host.ReferenceHub);
        toyInstance.NetworkPosition = position;
        toyInstance.NetworkRotation = spawnRotation;
        toyInstance.NetworkScale = spawnScale;
        toyInstance.NetworkMovementSmoothing = smoothing;
        toyInstance.NetworkIsStatic = isStatic;
        
        toyInstance.NetworkPrimitiveType = primitiveType;
        toyInstance.NetworkMaterialColor = materialColor ?? Color.white;
        toyInstance.NetworkPrimitiveFlags = primitiveFlags;

        var primitive = EntityManager.GetOrException<IPrimitive>(toyInstance);
        if (doSpawn) primitive.Spawn();
        return primitive;
    }

    public IPrimitive CreatePrimitive(Vector3 position, Vector3 eulerAngles, Vector3? scale,
        PrimitiveType primitiveType = PrimitiveType.Sphere, Color? materialColor = null,
        PrimitiveFlags primitiveFlags = PrimitiveFlags.None, byte smoothing = 0, bool isStatic = false,
        Footprint? ownerFootprint = null, bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreatePrimitive(position, rotationQuaternion, scale, primitiveType, materialColor, primitiveFlags,
            smoothing, isStatic, ownerFootprint, doSpawn);
    }

    #endregion

    #region Shooting Target Factory

    public IShootingTarget CreateShootingTarget(ShootingTargetPrefabs prefabType, Vector3 position,
        Quaternion? rotation = null, Vector3? scale = null, byte smoothing = 0, bool isStatic = false,
        Footprint? ownerFootprint = null, bool doSpawn = true)
    {
        if (!Prefabs.ShootingTargets.TryGetValue(prefabType, out var shootingTargetToyPrefab))
            throw new NullReferenceException($"Prefabs.ShootingTargets[{prefabType}] not found");

        var spawnRotation = rotation ?? Quaternion.identity;
        var spawnScale = scale ?? Vector3.one;

        var toyInstance = Object.Instantiate(shootingTargetToyPrefab, position, spawnRotation);
        toyInstance.transform.localScale = spawnScale;

        toyInstance.SpawnerFootprint = ownerFootprint ?? new Footprint(Server.Host.ReferenceHub);
        toyInstance.NetworkPosition = position;
        toyInstance.NetworkRotation = spawnRotation;
        toyInstance.NetworkScale = spawnScale;
        toyInstance.NetworkMovementSmoothing = smoothing;
        toyInstance.NetworkIsStatic = isStatic;

        var shootingTarget = EntityManager.Get<IShootingTarget>(toyInstance);
        if (shootingTarget is null)
            throw new NullReferenceException(nameof(shootingTarget));

        if (doSpawn) shootingTarget.Spawn();
        return shootingTarget;
    }

    public IShootingTarget CreateShootingTarget(ShootingTargetPrefabs prefabType, Vector3 position, Vector3 eulerAngles,
        Vector3? scale = null, byte smoothing = 0, bool isStatic = false, Footprint? ownerFootprint = null,
        bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreateShootingTarget(prefabType, position, rotationQuaternion, scale, smoothing, isStatic,
            ownerFootprint, doSpawn);
    }

    #endregion
    
    #region Speaker Factory

    public ISpeaker CreateSpeaker(Vector3 position, byte controllerId = 0, bool isSpatial = true, float volume = 1.0F,
        float minDistance = 1.0F, float maxDistance = 15.0F, byte smoothing = 0, bool isStatic = false, Footprint? ownerFootprint = null, bool doSpawn = true)
    {
        if (Prefabs.Speaker == null)
            throw new NullReferenceException("Prefabs.Speaker not found");

        var toyInstance = Object.Instantiate(Prefabs.Speaker, position, Quaternion.identity);

        toyInstance.SpawnerFootprint = ownerFootprint ?? new Footprint(Server.Host.ReferenceHub);
        toyInstance.NetworkPosition = position;
        toyInstance.NetworkMovementSmoothing = smoothing;
        toyInstance.NetworkIsStatic = isStatic;
        
        toyInstance.NetworkControllerId = controllerId;
        toyInstance.NetworkIsSpatial = isSpatial;
        toyInstance.NetworkVolume = volume;
        toyInstance.NetworkMinDistance = minDistance;
        toyInstance.NetworkMaxDistance = maxDistance;

        var speaker = EntityManager.GetOrException<ISpeaker>(toyInstance);
        if (doSpawn) speaker.Spawn();
        return speaker;
    }

    #endregion
}