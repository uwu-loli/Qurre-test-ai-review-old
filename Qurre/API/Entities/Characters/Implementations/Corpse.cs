using PlayerRoles;
using PlayerRoles.Ragdolls;
using Qurre.API.Core;
using Qurre.API.Core.Implementations;
using Qurre.Internal.Attributes;

namespace Qurre.API.Entities.Characters.Implementations;

[EntityWrapBindForFactory(typeof(BasicRagdoll))]
internal sealed class Corpse : NetworkEntity, ICorpse
{
    private Player _owner;

    public Corpse(BasicRagdoll corpseBase) : base(corpseBase.gameObject)
    {
        Base = corpseBase;
        _owner = Player.Get(Base.Instance.NetworkInfo.OwnerHub) ?? Server.Host;
    }

    /// <inheritdoc />
    public UnityObjectWrapper<BasicRagdoll> Base { get; }

    /// <inheritdoc />
    public RagdollData RagdollInfo
    {
        get => Base.Instance.NetworkInfo;
        set => Base.Instance.NetworkInfo = value;
    }

    /// <inheritdoc />
    public float Lifetime => Base.Instance._existenceTime;

    /// <inheritdoc />
    public string CharacterName
    {
        get => RagdollInfo.Nickname;
        set => RagdollInfo = RagdollInfo.CopyWithReplace(
            newNickname: value,
            curPosition: WorldPosition,
            curRotation: WorldRotation
        );
    }

    /// <inheritdoc />
    public RoleTypeId Role
    {
        get => RagdollInfo.RoleType;
        set => RagdollInfo = RagdollInfo.CopyWithReplace(
            newRole: value,
            curPosition: WorldPosition,
            curRotation: WorldRotation);
    }

    /// <inheritdoc />
    public Player Owner
    {
        get => _owner;
        set
        {
            _owner = value;
            RagdollInfo = RagdollInfo.CopyWithReplace(
                value.ReferenceHub,
                curPosition: WorldPosition,
                curRotation: WorldRotation);
        }
    }

    private void UpdateRagdollInfo()
    {
        RagdollInfo = new RagdollData(RagdollInfo.OwnerHub, RagdollInfo.Handler, WorldPosition,
            WorldRotation, RagdollInfo.Serial);
    }

    protected override void OnPositionChanged()
    {
        UpdateRagdollInfo();
        base.OnPositionChanged();
    }

    protected override void OnRotationChanged()
    {
        UpdateRagdollInfo();
        base.OnRotationChanged();
    }
}