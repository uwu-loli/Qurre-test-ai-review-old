using JetBrains.Annotations;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using Qurre.API.Core;

namespace Qurre.API.Entities.Characters;

[PublicAPI]
public interface ICorpse : INetworkEntity
{
    UnityObjectWrapper<BasicRagdoll> Base { get; }

    RagdollData RagdollInfo { get; set; }

    float Lifetime { get; }

    string CharacterName { get; set; }

    RoleTypeId Role { get; set; }

    Player Owner { get; set; }
}