using JetBrains.Annotations;
using PlayerRoles.PlayableScps.Scp106;
using PlayerRoles.Subroutines;
using Qurre.API.Core;
using Scp106RoleBase = PlayerRoles.PlayableScps.Scp106.Scp106Role;

namespace Qurre.API.Entities.Characters.Components.Roles;

[PublicAPI]
public sealed class Scp106 : Role
{
    internal Scp106(Scp106RoleBase scp106RoleBase) : base(scp106RoleBase)
    {
        Base = scp106RoleBase;
        SubroutineManager = scp106RoleBase.SubroutineModule;

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp106Attack attack))
            Log.Error("[Roles > Scp106] >> Scp106Attack is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp106SinkholeController sinkholeController))
            Log.Error("[Roles > Scp106] >> Scp106SinkholeController is null");

        if (!SubroutineManager.Instance.TryGetSubroutine(out Scp106StalkAbility stalkAbility))
            Log.Error("[Roles > Scp106] >> Scp106StalkAbility is null");

        Attack = attack;
        SinkholeController = sinkholeController;
        StalkAbility = stalkAbility;
    }

    public new UnityObjectWrapper<Scp106RoleBase> Base { get; }

    public UnityObjectWrapper<SubroutineManagerModule> SubroutineManager { get; }

    public UnityObjectWrapper<Scp106Attack> Attack { get; }
    public UnityObjectWrapper<Scp106SinkholeController> SinkholeController { get; }
    public UnityObjectWrapper<Scp106StalkAbility> StalkAbility { get; }
}