using System.Collections.Generic;
using JetBrains.Annotations;
using PlayerRoles.PlayableScps.Scp173;
using PlayerRoles.Subroutines;
using Qurre.API.Core;
using Scp173RoleBase = PlayerRoles.PlayableScps.Scp173.Scp173Role;

namespace Qurre.API.Entities.Characters.Components.Roles;

[PublicAPI]
public sealed class Scp173 : Role
{
    internal Scp173(Scp173RoleBase scp173RoleBase) : base(scp173RoleBase)
    {
        Base = scp173RoleBase;
        SubroutineManager = scp173RoleBase.SubroutineModule;

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp173ObserversTracker observersTracker))
            Log.Error("[Roles > Scp173] >> Scp173ObserversTracker is null");

        ObserversTracker = observersTracker;
    }

    public static HashSet<Player> IgnoredPlayers { get; } = [];

    #region Properties

    public new UnityObjectWrapper<Scp173RoleBase> Base { get; }

    public UnityObjectWrapper<SubroutineManagerModule> SubroutineManager { get; }

    public UnityObjectWrapper<Scp173ObserversTracker> ObserversTracker { get; }

    #endregion
}