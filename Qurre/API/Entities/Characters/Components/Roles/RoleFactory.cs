using JetBrains.Annotations;
using RoleBase = PlayerRoles.PlayerRoleBase;
using Scp079RoleBase = PlayerRoles.PlayableScps.Scp079.Scp079Role;
using Scp096RoleBase = PlayerRoles.PlayableScps.Scp096.Scp096Role;
using Scp106RoleBase = PlayerRoles.PlayableScps.Scp106.Scp106Role;
using Scp173RoleBase = PlayerRoles.PlayableScps.Scp173.Scp173Role;
using HumanRoleBase = PlayerRoles.HumanRole;

namespace Qurre.API.Entities.Characters.Components.Roles;

[PublicAPI]
public static class RoleFactory
{
    public static Role Create(RoleBase roleBase)
    {
        return roleBase switch
        {
            Scp079RoleBase scp079Base => new Scp079(scp079Base),
            Scp096RoleBase scp096Base => new Scp096(scp096Base),
            Scp106RoleBase scp106Base => new Scp106(scp106Base),
            Scp173RoleBase scp173Base => new Scp173(scp173Base),
            HumanRoleBase humanBase => new HumanRole(humanBase),
            _ => null!
        };
    }
}