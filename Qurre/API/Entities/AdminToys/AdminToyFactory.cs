using System;
using System.Collections.Generic;
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

    public ILightPoint CreateLightPoint(Vector3 position,
        Quaternion? rotation = null,
        Color? color = null,
        LightType lightType = LightType.Point,
        float range = 10.0F,
        float intensity = 1.0F,
        float shadowStrength = 0.0F,
        LightShadows shadowType = LightShadows.Hard,
        float spotAngle = 30.0F,
        float spotInnerAngle = 0.0F,
        byte smoothing = 0,
        bool isStatic = false,
        Footprint? ownerFootprint = null,
        bool doSpawn = true)
    {
        if (Prefabs.Light == null)
            throw new NullReferenceException("Prefabs.LightPoint is null.");

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

        var lightPoint = EntityManager.GetOrException<ILightPoint>(toyInstance);
        if (doSpawn) lightPoint.Spawn();
        else lightPoint.UnSpawn();
        return lightPoint;
    }

    public ILightPoint CreateLightPoint(Vector3 position,
        Vector3 eulerAngles,
        Color? color = null,
        LightType lightType = LightType.Spot,
        float range = 10.0F,
        float intensity = 1.0F,
        float shadowStrength = 0.0F,
        LightShadows shadowType = LightShadows.Hard,
        float spotAngle = 30.0F,
        float spotInnerAngle = 0.0F,
        byte smoothing = 0,
        bool isStatic = false,
        Footprint? ownerFootprint = null,
        bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreateLightPoint(position, rotationQuaternion, color, lightType, range, intensity, shadowStrength,
            shadowType,
            spotAngle, spotInnerAngle, smoothing, isStatic, ownerFootprint, doSpawn);
    }

    #endregion

    #region Primitive Factory

    public IPrimitive CreatePrimitive(Vector3 position,
        Quaternion? rotation = null,
        Vector3? scale = null,
        PrimitiveType primitiveType = PrimitiveType.Sphere,
        Color? materialColor = null,
        PrimitiveFlags primitiveFlags = PrimitiveFlags.Visible | PrimitiveFlags.Collidable,
        byte smoothing = 0,
        bool isStatic = false,
        Footprint? ownerFootprint = null,
        bool doSpawn = true)
    {
        if (Prefabs.Primitive == null)
            throw new NullReferenceException("Prefabs.Primitive is null.");

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
        else primitive.UnSpawn();
        return primitive;
    }

    public IPrimitive CreatePrimitive(Vector3 position,
        Vector3 eulerAngles,
        Vector3? scale,
        PrimitiveType primitiveType = PrimitiveType.Sphere,
        Color? materialColor = null,
        PrimitiveFlags primitiveFlags = PrimitiveFlags.Visible | PrimitiveFlags.Collidable,
        byte smoothing = 0,
        bool isStatic = false,
        Footprint? ownerFootprint = null,
        bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreatePrimitive(position, rotationQuaternion, scale, primitiveType, materialColor, primitiveFlags,
            smoothing, isStatic, ownerFootprint, doSpawn);
    }

    #endregion

    #region Shooting Target Factory

    public IShootingTarget CreateShootingTarget(ShootingTargetPrefabs prefab,
        Vector3 position,
        Quaternion? rotation = null,
        Vector3? scale = null,
        byte smoothing = 0,
        bool isStatic = false,
        Footprint? ownerFootprint = null,
        bool doSpawn = true)
    {
        if (!Prefabs.ShootingTargets.TryGetValue(prefab, out var shootingTargetToyPrefab))
            throw new KeyNotFoundException($"Prefabs.ShootingTargets[{prefab}] not found.");

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

        var shootingTarget = EntityManager.GetOrException<IShootingTarget>(toyInstance);
        if (doSpawn) shootingTarget.Spawn();
        else shootingTarget.UnSpawn();
        return shootingTarget;
    }

    public IShootingTarget CreateShootingTarget(ShootingTargetPrefabs prefab,
        Vector3 position,
        Vector3 eulerAngles,
        Vector3? scale = null,
        byte smoothing = 0,
        bool isStatic = false,
        Footprint? ownerFootprint = null,
        bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreateShootingTarget(prefab, position, rotationQuaternion, scale, smoothing, isStatic,
            ownerFootprint, doSpawn);
    }

    #endregion
    
    #region Speaker Factory

    public ISpeaker CreateSpeaker(Vector3 position,
        byte controllerId = 0,
        bool isSpatial = true,
        float volume = 1.0F,
        float minDistance = 1.0F,
        float maxDistance = 15.0F,
        byte smoothing = 0,
        bool isStatic = false,
        Footprint? ownerFootprint = null,
        bool doSpawn = true)
    {
        if (Prefabs.Speaker == null)
            throw new NullReferenceException("Prefabs.Speaker is null.");

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
        else speaker.UnSpawn();
        return speaker;
    }

    #endregion
}