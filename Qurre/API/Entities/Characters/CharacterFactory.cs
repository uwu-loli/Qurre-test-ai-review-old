using System;
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
    
    public ICorpse CreateCorpse(RoleTypeId roleType, string nickname = "Dummy", Vector3? position = null,
        Quaternion? rotation = null, Vector3? scale = null, string customDeathReason = "Died to a heart attack.",
        DamageHandlerBase? damageHandler = null, Player? owner = null, bool doSpawn = true)
    {
        if (!PlayerRoleLoader.AllRoles.TryGetValue(roleType, out var role))
            throw new Exception($"RoleTypeId not found: {roleType}");

        if (role is not IRagdollRole ragdollRole)
            throw new MissingComponentException("IRagdollRole component not found");

        var spawnPosition = position ?? Vector3.zero;
        var spawnRotation = rotation ?? Quaternion.identity;
        var spawnScale = scale ?? Vector3.one;
        
        var ragdollInstance = Object.Instantiate(ragdollRole.Ragdoll, spawnPosition, spawnRotation);
        ragdollInstance.transform.localScale = spawnScale;
        
        var referenceHub = owner?.ReferenceHub ?? Server.Host.ReferenceHub;
        var corpse = EntityManager.GetOrException<ICorpse>(ragdollInstance);
        corpse.Base.Instance.NetworkInfo = new RagdollData(referenceHub,
            damageHandler ?? new CustomReasonDamageHandler(customDeathReason),
            roleType,
            spawnPosition,
            spawnRotation,
            nickname,
            NetworkTime.time);

        if (doSpawn) corpse.Spawn();
        return corpse;
    }

    public ICorpse CreateCorpse(RoleTypeId roleType, Player owner, Vector3? position = null,
        Quaternion? rotation = null, Vector3? scale = null, string customDeathReason = "Died to a heart attack.",
        DamageHandlerBase? damageHandler = null)
    {
        return CreateCorpse(roleType, owner.UserInformation.Nickname, position, rotation, scale, customDeathReason,
            damageHandler, owner);
    }

    public ICorpse CreateCorpse(Player owner, Vector3? position = null, Quaternion? rotation = null, Vector3? scale = null,
        string customDeathReason = "Died to a heart attack.", DamageHandlerBase? damageHandler = null)
    {
        return CreateCorpse(owner.RoleInformation.RoleType, owner, position, rotation, scale, customDeathReason,
            damageHandler);
    }
    
    #endregion
}