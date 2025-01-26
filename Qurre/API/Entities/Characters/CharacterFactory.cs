using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Qurre.API.Entities.Characters;

[PublicAPI]
public class CharacterFactory
{
    #region Corpse Factory
    
    public ICorpse CreateCorpse(RoleTypeId roleType,
        Vector3 position,
        Quaternion? rotation = null,
        Vector3? scale = null,
        string nickname = "Dummy",
        string customDeathReason = "Died to a heart attack.",
        DamageHandlerBase? damageHandler = null,
        Player? owner = null,
        bool doSpawn = true)
    {
        if (!PlayerRoleLoader.AllRoles.TryGetValue(roleType, out var role))
            throw new KeyNotFoundException($"RoleTypeId not found: {roleType}.");

        if (role is not IRagdollRole ragdollRole)
            throw new MissingComponentException("IRagdollRole component not found.");

        var spawnRotation = rotation ?? Quaternion.identity;
        var spawnScale = scale ?? Vector3.one;
        
        var ragdollInstance = Object.Instantiate(ragdollRole.Ragdoll, position, spawnRotation);
        ragdollInstance.transform.localScale = spawnScale;
        
        var referenceHub = owner?.ReferenceHub ?? Server.Host.ReferenceHub;
        var corpse = EntityManager.GetOrException<ICorpse>(ragdollInstance);
        corpse.Base.Instance.NetworkInfo = new RagdollData(referenceHub,
            damageHandler ?? new CustomReasonDamageHandler(customDeathReason),
            roleType,
            position,
            spawnRotation,
            nickname,
            NetworkTime.time);

        if (doSpawn) corpse.Spawn();
        else corpse.UnSpawn();
        return corpse;
    }

    public ICorpse CreateCorpse(RoleTypeId roleType,
        Vector3 position,
        Vector3 eulerAngles,
        Vector3? scale = null,
        string nickname = "Dummy",
        string customDeathReason = "Died to a heart attack.",
        DamageHandlerBase? damageHandler = null,
        Player? owner = null,
        bool doSpawn = true)
    {
        var rotationQuaternion = Quaternion.Euler(eulerAngles);
        return CreateCorpse(roleType, position, rotationQuaternion, scale, nickname, customDeathReason, damageHandler,
            owner, doSpawn);
    }

    #endregion
}