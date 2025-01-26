using JetBrains.Annotations;
using Qurre.API.Core;
using Qurre.API.Exceptions;
using HumanRoleBase = PlayerRoles.HumanRole;

namespace Qurre.API.Entities.Characters.Components.Roles;

[PublicAPI]
public sealed class HumanRole : Role
{
    internal HumanRole(HumanRoleBase humanRoleBase) : base(humanRoleBase)
    {
        Base = humanRoleBase;
    }

    public new UnityObjectWrapper<HumanRoleBase> Base { get; }

    /// <exception cref="ObjectDestroyedException" />
    public bool IsUnitNamesReady => Base.Instance.UsesUnitNames;

    /// <exception cref="ObjectDestroyedException" />
    public byte UnitNameId
    {
        get => Base.Instance.UnitNameId;
        set => Base.Instance.UnitNameId = value;
    }
}