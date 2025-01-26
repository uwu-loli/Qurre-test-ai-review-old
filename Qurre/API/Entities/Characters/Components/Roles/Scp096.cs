using JetBrains.Annotations;
using PlayerRoles.PlayableScps.Scp096;
using PlayerRoles.Subroutines;
using Qurre.API.Core;
using Scp096RoleBase = PlayerRoles.PlayableScps.Scp096.Scp096Role;

namespace Qurre.API.Entities.Characters.Components.Roles;

[PublicAPI]
public sealed class Scp096 : Role
{
    internal Scp096(Scp096RoleBase scp097RoleBase) : base(scp097RoleBase)
    {
        Base = scp097RoleBase;
        SubroutineManager = scp097RoleBase.SubroutineModule;

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp096RageManager rageManager))
            Log.Error("[Roles > Scp096] >> Scp096RageManager is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp096TargetsTracker targetsTracker))
            Log.Error("[Roles > Scp096] >> Scp096TargetsTracker is null");

        RageManager = rageManager;
        TargetsTracker = targetsTracker;
    }

    public new UnityObjectWrapper<Scp096RoleBase> Base { get; }

    public UnityObjectWrapper<SubroutineManagerModule> SubroutineManager { get; }

    public UnityObjectWrapper<Scp096RageManager> RageManager { get; }

    public UnityObjectWrapper<Scp096TargetsTracker> TargetsTracker { get; }
}