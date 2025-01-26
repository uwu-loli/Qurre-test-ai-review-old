using JetBrains.Annotations;
using PlayerRoles;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using UnityEngine;
using RoleBase = PlayerRoles.PlayerRoleBase;

namespace Qurre.API.Entities.Characters.Components.Roles;

[PublicAPI]
public abstract class Role(RoleBase roleBase)
{
    public UnityObjectWrapper<RoleBase> Base { get; } = roleBase;

    /// <exception cref="ObjectDestroyedException" />
    public Player Player { get; } = Player.Get(roleBase.GetComponentInParent<ReferenceHub>()) ??
                                    throw new ObjectDestroyedException();

    /// <exception cref="ObjectDestroyedException" />
    public RoleTypeId RoleType => Base.Instance.RoleTypeId;

    /// <exception cref="ObjectDestroyedException" />
    public Team Team => Base.Instance.Team;

    /// <exception cref="ObjectDestroyedException" />
    public Color Color => Base.Instance.RoleColor;

    /// <exception cref="ObjectDestroyedException" />
    public string Name => Base.Instance.RoleName;

    /// <exception cref="ObjectDestroyedException" />
    public string ColoredName => Base.Instance.GetColoredName();

    /// <exception cref="ObjectDestroyedException" />
    public float ActiveTime => Base.Instance.ActiveTime;
}